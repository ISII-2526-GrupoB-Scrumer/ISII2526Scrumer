using System;

public class Review
{
    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    public DateTime Created { get; set; }

    [Key]
    public int Id { get; set; }

    public IList<ReviewItem> Cars { get; set; }

    
    public ApplicationUser Client { get; set; }

    public Review()
    {
    }

    public Review(int id, DateTime created)
    {
        Id = id;
        Created = created;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}