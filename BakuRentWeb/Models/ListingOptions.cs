namespace BakuRentWeb.Models
{
    public static class ListingOptions
    {
        public static readonly string[] Districts =
        {
            "Nəsimi", "Nərimanov", "Yasamal", "Xətai", "Səbail",
            "Binəqədi", "Nizami", "Sabunçu", "Suraxanı", "Xəzər",
            "Qaradağ", "Pirallahı"
        };

        public static string StatusLabel(string status) => status switch
        {
            "Active" => "Aktiv",
            "Rented" => "Kirayə verilib",
            _ => status
        };
    }
}
