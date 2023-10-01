using System.ComponentModel.DataAnnotations;

namespace TsdDelivery.Application.Models.Coordinates;

public class DestinationCoordinates
{
    [Range(-90, 90, ErrorMessage = "Invalid latitude value")]
    public double? LatitudeDes { get; set; }
    
    [Range(-180, 180, ErrorMessage = "Invalid longitude value")]
    public double? LongitudeDes { get; set; }
}