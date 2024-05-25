
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace cp_randomcard.Entities
{
    public class Card
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id {  get; set; }
        [Required]
        public String Title { get; set; }
        [Required]
        public String Atribute { get; set; }
        [Required]
        public int Power { get; set; }
        [Required]
        public int Health { get; set; }
        public Card() { }
        public Card(string title, string atribute, int power, int health)
        {
            Title = title;
            Atribute = atribute;
            Power = power;
            Health = health;
        }
    }
}
