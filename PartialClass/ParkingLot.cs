using Microsoft.AspNetCore.Mvc;
using MyGoParking.Metadata;

namespace MyGoParking.Models;

[ModelMetadataType(typeof(ParkingLotMetadata))]
public partial class ParkingLot
{
}
