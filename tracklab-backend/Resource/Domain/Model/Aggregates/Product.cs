using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using TrackLab.Shared.Domain.ValueObjects;
using Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public partial class Product
{
    public long Id { get; private set; }
    public long TenantId { get; private set; }
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Tenant { get; set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Price Price { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public int Stock { get; private set; }

    private static readonly HashSet<string> AllowedCategories = new() { "Tecnología", "Materiales", "Equipos" };

    // Constructor requerido por EF Core
    public Product() { }

    // Constructor de dominio
    public Product(CreateProductCommand command)
    {
        // Validación de Name: no puede ser vacío ni solo números
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new NullOrEmptyProductNameException();
        if (Regex.IsMatch(command.Name, @"^\d+$"))
            throw new InvalidProductNameException("Product name cannot be only numbers.");
        if (string.IsNullOrWhiteSpace(command.Category))
            throw new ArgumentException("Category cannot be null or empty.", nameof(command.Category));
        if (!AllowedCategories.Contains(command.Category))
            throw new ArgumentException("Category must be one of: Tecnología, Materiales, Equipos.", nameof(command.Category));
        if (command.Stock < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(command.Stock));
        Name = command.Name;
        Description = command.Description;
        Category = command.Category;
        Stock = command.Stock;
        try
        {
            Price = new Price(command.PriceAmount, command.PriceCurrency);
        }
        catch (ArgumentException ex) when (ex.ParamName == "amount")
        {
            throw new InvalidProductPriceAmountException(ex.Message, ex);
        }
        catch (ArgumentException ex) when (ex.ParamName == "currency")
        {
            throw new InvalidProductPriceCurrencyException(ex.Message, ex);
        }
        // El TenantId se establecerá desde el contexto actual
    }

    public void UpdateInfo(UpdateProductInfoCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new NullOrEmptyProductNameException();
        if (Regex.IsMatch(command.Name, @"^\d+$"))
            throw new InvalidProductNameException("Product name cannot be only numbers.");
        if (string.IsNullOrWhiteSpace(command.Category))
            throw new ArgumentException("Category cannot be null or empty.", nameof(command.Category));
        if (!AllowedCategories.Contains(command.Category))
            throw new ArgumentException("Category must be one of: Tecnología, Materiales, Equipos.", nameof(command.Category));
        if (command.Stock < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(command.Stock));
        Name = command.Name;
        Description = command.Description;
        Category = command.Category;
        Stock = command.Stock;
        try
        {
            Price = new Price(command.PriceAmount, command.PriceCurrency);
        }
        catch (ArgumentException ex) when (ex.ParamName == "amount")
        {
            throw new InvalidProductPriceAmountException(ex.Message, ex);
        }
        catch (ArgumentException ex) when (ex.ParamName == "currency")
        {
            throw new InvalidProductPriceCurrencyException(ex.Message, ex);
        }
    }

    public void SetTenantId(long tenantId)
    {
        TenantId = tenantId;
    }
} 