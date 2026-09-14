using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class DashboardTelemetryWriter(
    ILogger<DashboardTelemetryWriter> logger) : IDashboardTelemetryWriter
{
    public void Write(DashboardRunTelemetry telemetry)
    {
        logger.LogInformation(
            "Feature={FeatureName} PromptVersion={PromptVersion} Provider={Provider} DurationMs={DurationMs} UsedFallback={UsedFallback} InputCharacters={InputCharacters} OutputCharacters={OutputCharacters} ValidationFailureReason={ValidationFailureReason}",
            telemetry.FeatureName,
            telemetry.PromptVersion,
            telemetry.Provider,
            telemetry.DurationMs,
            telemetry.UsedFallback,
            telemetry.InputCharacters,
            telemetry.OutputCharacters,
            telemetry.ValidationFailureReason);
    }
}
