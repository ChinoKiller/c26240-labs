using backend_lab_C26240.Models;
using backend_lab_C26240.Repositories;

namespace backend_lab_C26240.Services
{
    public class CountryService
    {
        private readonly CountryRepository _countryRepository;

        public CountryService(CountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<CountryModel> GetCountries()
        {
            return _countryRepository.GetCountries();
        }

        public bool CreateCountry(CountryModel country)
        {
            return _countryRepository.CreateCountry(country);
        }
        public bool DeleteCountry(int id)
        {
            return _countryRepository.DeleteCountry(id);
        }
    }
}