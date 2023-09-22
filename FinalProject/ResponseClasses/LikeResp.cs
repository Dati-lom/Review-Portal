using FinalProject.Models;

namespace FinalProject.ResponseClasses;

public class LikeResp
{
    public bool IsLikedByThisUser { set; get; }
    public Comment Comment { set; get; }
}