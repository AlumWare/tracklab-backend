namespace TrackLab.IAM.Infrastructure.Pipeline.Middleware.Attributes;

/// <summary>
/// This attribute is used to decorate controllers and actions that do not require authorization.
/// It skips authorization if the action is decorated with [AllowAnonymous] attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{
}