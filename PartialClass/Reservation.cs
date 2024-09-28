using Microsoft.AspNetCore.Mvc;
using MyGoParking.Metadata;

namespace MyGoParking.Models
{
    [ModelMetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    {
    }
}
