using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.MonthlyBillings;

/// <summary>
/// Informations about new expense.
/// </summary>
/// <param name="Money">Amount of the expense.</param>
/// <param name="Currency">Currency of the expense.</param>
/// <param name="ExpenseDate">When the expense happened.</param>
/// <param name="Description">Short description about the expense.</param>
[SwaggerSchemaFilter(typeof(AddExpenseRequestFilter))]
public sealed record AddExpenseRequest(
    decimal Money,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description
);

public class AddExpenseRequestFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject()
        {
            ["Money"] = new OpenApiDouble(256.97),
            ["Currency"] = new OpenApiString("PLN"),
            ["ExpenseDate"] = new OpenApiDateTime(new DateTimeOffset(new DateTime(2023, 09, 13, 15, 34, 11))),
            ["Description"] = new OpenApiString("Medicines")
        };
    }
}
