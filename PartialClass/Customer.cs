using Microsoft.AspNetCore.Mvc;
using MyGoParking.Metadata;

namespace MyGoParking.Models;
[ModelMetadataType(typeof(CustomerMetadata))]
public partial class Customer
{
}

