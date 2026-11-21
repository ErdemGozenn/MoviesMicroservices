using System.Globalization;
using Movies.Core.Models;

namespace Movies.Core.Services;

public abstract class Service
{
    private string _culture;

    protected string Culture
    {
        get => _culture;
        set
        {
            _culture = value;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(_culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_culture);
        }
    }

    protected Service()
    {
        Culture = "en-US";
    }

    protected CommandResponse Success(int id, string message = default)
        => new(true, message ?? "Operation successful.", id);

    protected CommandResponse Error(string message = default)
        => new(false, message ?? "Operation failed!");
}