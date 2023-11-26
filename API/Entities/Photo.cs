using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }
    // by explicitly setting an AppUser and AppUserId for every Photo
    // when we run our Database Migration, we will ensure that every photo is linked to an AppUser
    // and if that AppUser is deleted ... Cascade Delete will remove those photos
    // thus no "orphan" photos in our database
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}