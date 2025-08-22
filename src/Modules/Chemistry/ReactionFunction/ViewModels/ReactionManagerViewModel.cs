using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Modules.Teacher.Chemistry.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Services.Chemistry;
using System.Collections.ObjectModel;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IChemistryService _chemistryService; // Assuming you have a service for chemistry operations
        private readonly ChemistryDataCache _chemistryDataCache; // Assuming you have a cache for chemistry data

        private string _reactants;
        private string _products;

        private ObservableCollection<Reaction> _allReactions;
        private ObservableCollection<Reaction> _reactions;

        #region Properties
        public string Reactants
        {
            get => _reactants;
            set
            {
                _reactants = value;
                OnPropertyChanged();
            }
        }
        public string Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Reaction> Reactions
        {
            get => _reactions;
            set
            {
                _reactions = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AddReactionCommand { get; set; } // Command to open the add reaction window
        public ICommand SearchReactionCommand { get; set; } // Command to search for reactions
        public ICommand SelectReactionCommand { get; set; } // Command to select a reaction from search results
        #endregion

        /// <summary>
        /// Constructor for ReactionManagerViewModel
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ReactionManagerViewModel(IServiceProvider serviceProvider,
                                        IChemistryService chemistryService,
                                        ChemistryDataCache chemistryDataCache)
        {
            _serviceProvider = serviceProvider;
            _chemistryService = chemistryService;
            _chemistryDataCache = chemistryDataCache;

            _reactions = new ObservableCollection<Reaction>();
            _allReactions = new ObservableCollection<Reaction>();

            AddReactionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open the add reaction window
                var window = _serviceProvider.GetRequiredService<AddReactionWindow>();

                if (window.DataContext is ReactionViewModel vm && vm is IAsyncInitializable init)
                {
                    // Initialize the add reaction window asynchronously
                    _ = init.InitializeAsync();
                }

                window.ShowDialog();

                _allReactions = new ObservableCollection<Reaction>(_chemistryDataCache.AllReactions);
            });

            SearchReactionCommand = new RelayCommand<object>((p) => true, (p) => SearchReaction());

            SelectReactionCommand = new RelayCommand<object>((p) => true, (p) => SelectReaction((Reaction)p));
        }

        /// <summary>
        /// Searches for reactions based on the provided reactants and products.
        /// </summary>
        private void SearchReaction()
        {
            // Validate the input for reactants and products
            if (string.IsNullOrWhiteSpace(Reactants) && string.IsNullOrWhiteSpace(Products))
            {
                MessageBox.Show("Vui lòng nhập ít nhất 1 chất tham gia hoặc chất sản phẩm", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Split the reactants and products strings into lists
            List<string> reactants = string.IsNullOrWhiteSpace(Reactants) ? new List<string>() : Reactants.Split(' ').ToList();
            List<string> products = string.IsNullOrWhiteSpace(Products) ? new List<string>() : Products.Split(' ').ToList();

            // Clear the current reactions collection
            Reactions?.Clear();

            // Search for reactions that match the given reactants and products
            var matches = _allReactions.AsParallel()
                                       .Where(r =>
                                       {
                                           var rReactants = r.Reactants.Select(x => x.Compound?.Formula ?? x.Element?.Formula ?? string.Empty)
                                                                       .Where(x => !string.IsNullOrWhiteSpace(x))
                                                                       .ToList();

                                           var rProducts = r.Products.Select(x => x.Compound?.Formula ?? x.Element?.Formula ?? string.Empty)
                                                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                                                     .ToList();

                                           bool reactantsMatch = !reactants.Any() || reactants.All(x => rReactants.Contains(x));
                                           bool productsMatch = !products.Any() || products.All(x => rProducts.Contains(x));

                                           return reactantsMatch && productsMatch;
                                       })
                                       .ToList();

            // Populate the Reactions collection with the matches found
            Reactions = new ObservableCollection<Reaction>(matches);

            if (!_allReactions.Any())
            {
                MessageBox.Show("Đang tải dữ liệu, vui lòng chờ, tải xong tự động tìm kiếm", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!Reactions.Any())
            {
                MessageBox.Show("Không tìm thấy phản ứng nào phù hợp với điều kiện tìm kiếm", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Selects a reaction from the search results and performs any necessary actions, such as displaying details or updating the UI.
        /// </summary>
        /// <param name="selectedReaction"></param>
        private void SelectReaction(Reaction selectedReaction)
        {
            var vm = new ReactionSelectionResultViewModel(selectedReaction);
            var window = new ReactionSelectionResultWindow
            {
                DataContext = vm
            };
            window.Show();
        }

        /// <summary>
        /// Initializes the view model asynchronously, loading necessary data and setting up the initial state.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            // Load initial data if needed
            await Task.Run(() =>
            {
                _chemistryDataCache.LoadAllData(_chemistryService);
                _allReactions = new ObservableCollection<Reaction>(_chemistryDataCache.AllReactions);
            }, cancellationToken);

            if (!string.IsNullOrWhiteSpace(Reactants) || !string.IsNullOrWhiteSpace(Products)) 
                SearchReaction();
        }
    }
}
