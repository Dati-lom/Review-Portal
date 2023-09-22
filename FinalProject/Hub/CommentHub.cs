using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Service;
using Microsoft.AspNetCore.SignalR;



public class CommentHub:Hub
{
    private readonly UserServices _userServices;

    public CommentHub(UserServices userServices)
    {
        _userServices = userServices;
    }

    public Task JoinGroup(string group)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    public async Task Comment(CommentDto commentDto)
    {
        await Clients.Groups($"review/{commentDto.ReviewId}").SendAsync("receiveComment",commentDto);
        _userServices.AddComment(commentDto);
    }
}