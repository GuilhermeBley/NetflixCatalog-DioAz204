using System.ComponentModel.DataAnnotations;

namespace NetflixCatalog_DioAz204.Func;

public class StorageAccountOptions
{
    [Required]
    [MinLength(2)]
    public string Container {  get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string ConnectionString { get; set; } = string.Empty;
}
