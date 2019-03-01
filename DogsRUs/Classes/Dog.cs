namespace DogsRUs.Classes
{
	public class Dog : NotifiableObject
	{
		public Dog(string name, int id)
		{
			ID = id;
			_breedName = name;
		}

		#region Fields

		private string _breedName;
		private string _subBreedName;
		private string _imageUrl;

		#endregion

		#region Properties

		public int ID { get; private set; }

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(_subBreedName))
					return _breedName;
				return _subBreedName + " " + _breedName;
			}
		}

		public string BreedName
		{
			get => _breedName;
			set
			{
				if (_breedName != value)
				{
					_breedName = value;
					OnNotifyPropertyChanged(nameof(BreedName));
				}
			}
		}

		public string SubBreedName
		{
			get => _subBreedName;
			set
			{
				if (_subBreedName != value)
				{
					_subBreedName = value;
					OnNotifyPropertyChanged(nameof(SubBreedName));
				}
			}
		}
		public string ImageUrl
		{
			get => _imageUrl;
			set
			{
				if (_imageUrl != value)
				{
					_imageUrl = value;
					OnNotifyPropertyChanged(nameof(ImageUrl));
				}
			}
		}

		#endregion
	}
}
