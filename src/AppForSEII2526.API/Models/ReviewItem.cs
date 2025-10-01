using System;

public class ReviewItem
{
    public int CarId { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public int ReviewId { get; set; }

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
