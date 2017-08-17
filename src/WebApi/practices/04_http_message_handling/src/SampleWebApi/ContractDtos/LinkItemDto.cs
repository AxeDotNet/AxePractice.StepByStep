namespace SampleWebApi.ContractDtos
{
    public class LinkItemDto
    {
        public LinkItemDto()
        {
        }

        public LinkItemDto(string @ref, string href, bool restricted)
        {
            Ref = @ref;
            Href = href;
            Restricted = restricted;
        }

        public string Ref { get; set; }
        public string Href { get; set; }
        public bool Restricted { get; set; }
    }
}