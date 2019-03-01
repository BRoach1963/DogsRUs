using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogsRUs.Classes;
using Json.Net;

namespace DogsRUs.ViewModels
{
	public class DogViewModel : NotifiableObject
	{

		public DogViewModel(Dog model)
		{
			_dogModel = model;
		}

		#region Fields

		private bool _showProgIndicator;
		private const string dogImageUrl1 = "https://dog.ceo/api/breed/";
		private const string dogImageUrl2 = "/images/random";
		private const string dogSubBreedUrl1 = "https://dog.ceo/api/breed/";
		private const string dogSubBreedUrl2 = "/list";
		private Dog _dogModel; 
		private List<string> _subBreeds;

		#endregion


		public Dog DogModel
		{
			get => _dogModel;

			set
			{
				if (_dogModel != value)
				{
					_dogModel = value; 
					OnNotifyPropertyChanged(nameof(DogModel));
				}
			}
		}

		public List<string> SubBreeds => _subBreeds;

		public bool HasSubBreeds => _subBreeds.Count > 0;

		public async void UpdateDogImage()
		{
			ShowProgressIndicator = true;
			string json = String.Empty;
			string url = string.Empty;
			if(string.IsNullOrEmpty(DogModel.SubBreedName))
				url = dogImageUrl1 + DogModel.BreedName + dogImageUrl2;
			else
			{
				url = dogImageUrl1 + DogModel.BreedName + "/" + DogModel.SubBreedName  + dogImageUrl2;
			}

			using (var webClient = new System.Net.WebClient())
			{
				json = await webClient.DownloadStringTaskAsync(new Uri(url));
			}
			var response = JsonNet.Deserialize<JsonUrlResponse>(json);

			DogModel.ImageUrl = response.message;
			ShowProgressIndicator = false;
		}

		public async Task DetermineIfTypeHasSubreeds()
		{
			string json = String.Empty;
			string url = dogSubBreedUrl1 + DogModel.Name + dogSubBreedUrl2;
			using (var webClient = new System.Net.WebClient())
			{
				json = await webClient.DownloadStringTaskAsync(new Uri(url));
			}
			var response = JsonNet.Deserialize<JsonListResponse>(json);

			if (response.message.Count > 0)
			{
				_subBreeds = response.message.ToList();
			}
			else
			{
				_subBreeds = new List<string>();
			}
		}

		public bool ShowProgressIndicator
		{
			get => _showProgIndicator;
			set
			{
				if (_showProgIndicator != value)
				{
					_showProgIndicator = value;
					OnNotifyPropertyChanged(nameof(ShowProgressIndicator));
				}
			}
		}
	}
}
