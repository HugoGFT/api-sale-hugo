namespace Ambev.DeveloperEvaluation.Domain.Dto.UserDto
{
    public class AddressDto
    {
        public string? Street { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public int? Number { get; set; }
        public string? ZipCode { get; set; } = string.Empty;
        public GeoLocationDto Geolocation { get; set; } = new GeoLocationDto();
    }
}
