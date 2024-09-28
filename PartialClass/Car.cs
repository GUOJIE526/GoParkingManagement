using Microsoft.AspNetCore.Mvc;
using MyGoParking.Metadata;

namespace MyGoParking.Models;
[ModelMetadataType(typeof(CarMetadata))]
public partial class Car
{
}
