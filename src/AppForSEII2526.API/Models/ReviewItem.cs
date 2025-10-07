using System;

public class ReviewItem
{
    [Required]
    public int CarId { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Range(1,5)]
    public int Rating { get; set; }

    [Required]
    public int ReviewId { get; set; }


    [ForeignKey("CarId")]
    public Car Car { get; set; }

    [ForeignKey("ReviewId")]
    public Review Review { get; set; }

    public ReviewItem()
    {
    }

    public ReviewItem(int carId, string description, int rating, int reviewId)
    {
        CarId = carId;
        Description = description;
        Rating = rating;
        ReviewId = reviewId;
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