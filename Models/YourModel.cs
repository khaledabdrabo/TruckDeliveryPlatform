using System.ComponentModel.DataAnnotations;

public class DeliveryModel
{
    [Display(Name = "رقم التوصيل")]
    public int DeliveryId { get; set; }

    [Display(Name = "اسم العميل")]
    [Required(ErrorMessage = "الرجاء إدخال اسم العميل")]
    public string CustomerName { get; set; }

    [Display(Name = "عنوان التوصيل")]
    [Required(ErrorMessage = "الرجاء إدخال عنوان التوصيل")]
    public string DeliveryAddress { get; set; }
} 