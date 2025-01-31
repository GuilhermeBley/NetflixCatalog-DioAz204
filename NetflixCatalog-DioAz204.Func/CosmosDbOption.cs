using System.ComponentModel.DataAnnotations;

namespace NetflixCatalog_DioAz204.Func;

public class CosmosDbOption
{
    [Required]
    [MinLength(10)]
    public string Endpoint { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string AccountKey { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string DataBaseName { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string ConnectionString { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string Container { get; set; } = string.Empty;
}
