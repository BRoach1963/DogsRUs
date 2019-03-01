using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using DogsRUs.Classes;
using Json.Net;

namespace DogsRUs.ViewModels
{
	public class DogsViewModel : NotifiableObject
	{
		#region Fields

		private const string breedsListUrl = "https://dog.ceo/api/breeds/list";
		private const string dogImageUrl1 = "https://dog.ceo/api/breed/";
		private const string dogImageUrl2 = "/images/random";
		private readonly ObservableCollection<DogViewModel> _fulldogsList = new ObservableCollection<DogViewModel>();
		private ObservableCollection<DogViewModel> _filteredDogList = new ObservableCollection<DogViewModel>();
		private readonly ObservableCollection<string> _subBreedList = new ObservableCollection<string>() { "All Main Breeds" };
		private DogViewModel _currentSelection;
		private string _currentFilter;
		private DelegateCommand _createDogListCommand;


		#endregion
		public DogsViewModel() => InitializeDogsList();

		private async Task InitializeDogsList()
		{
			var dogs = await GetDogInformationFromWebsite();
			await CreateDogsList(dogs);
			CreateFilteredDogListCommand.Execute(this);
		}

		private async Task<List<string>> GetDogInformationFromWebsite()
		{
			string json;
			using (var webClient = new System.Net.WebClient())
			{
				json = await webClient.DownloadStringTaskAsync(new Uri(breedsListUrl));
			}
			var response = JsonNet.Deserialize<JsonListResponse>(json);

			return response.message.ToList();
		}

		private async Task CreateDogsList(List<string> dogList)
		{
			for (int i = 0; i < dogList.Count; i++)
			{
				string dog = dogList[i] as string;
				_fulldogsList.Add(new DogViewModel(new Dog(dog, i)));
			}

			await CreateSubBreedsList();
			CurrentFilter = _subBreedList[0];
		}

		private async Task CreateSubBreedsList()
		{
			foreach (DogViewModel dog in _fulldogsList)
			{
				await dog.DetermineIfTypeHasSubreeds();
				if (dog.HasSubBreeds)
					_subBreedList.Add(dog.DogModel.Name);
			}
		}

		#region Properties

		public ObservableCollection<string> DogsWithSubreeds => _subBreedList;

		public ObservableCollection<DogViewModel> FilteredDogList => _filteredDogList;

		public DogViewModel CurrentSelection
		{
			get => _currentSelection;
			set
			{
				if (_currentSelection != value)
				{
					_currentSelection = value;
					_currentSelection?.UpdateDogImage();
					OnNotifyPropertyChanged(nameof(CurrentSelection));
				}
			}
		}

		public string CurrentFilter
		{
			get => _currentFilter;
			set
			{
				if (_currentFilter != value)
				{
					_currentFilter = value;
					OnNotifyPropertyChanged(nameof(CurrentFilter));
					CreateFilteredDogListCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public DelegateCommand CreateFilteredDogListCommand
		{
			get
			{
				if (_createDogListCommand == null)
					_createDogListCommand = new DelegateCommand(OnCreateDogListExecuted, CanExecuteCreateDogList);
				return _createDogListCommand;
			}
		}

		private bool CanExecuteCreateDogList()
		{
			return !string.IsNullOrEmpty(CurrentFilter);
		}

		private void OnCreateDogListExecuted()
		{
			if (CurrentFilter == "All Main Breeds")
			{
				CreateFilteredListFromFullList();
			}
			else
			{
				CreateFilteredDogListFromCurrentFilter();
			}
			OnNotifyPropertyChanged(nameof(FilteredDogList));
			CurrentSelection = FilteredDogList[0];
		}

		private void CreateFilteredListFromFullList()
		{
			FilteredDogList.Clear();
			foreach (var dog in _fulldogsList)
			{
				FilteredDogList.Add(dog);
			}
		}

		private void CreateFilteredDogListFromCurrentFilter()
		{
			FilteredDogList.Clear();
			var mainDog = _fulldogsList.FirstOrDefault(x => x.DogModel.Name == CurrentFilter);
			int index = 0;
			if (mainDog != null)
			{
				foreach (string name in mainDog.SubBreeds)
				{
					FilteredDogList.Add(new DogViewModel(new Dog(mainDog.DogModel.Name, index)
					{
						SubBreedName = name
					}));
					index++;
				}
			}
			else
			{
				FilteredDogList.Add(new DogViewModel(new Dog("No Dogs Found", -1)));
			}

		}

		#endregion
	}
}
