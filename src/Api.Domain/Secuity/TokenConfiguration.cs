namespace Api.Domain.Secuity
{
    public class TokenConfiguration
    {

     public string Audience { get; set; }

     // -- Emissor
      public string Issuer { get; set; }

      public int Seconds { get; set; }

    }
}