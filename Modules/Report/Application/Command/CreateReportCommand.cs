namespace liminal_api.Modules.Report.Application.Command
{
    public record CreateReportCommand(
        string AuthorId,
        string Title,
        string Nivel,
        int LevelClassId,
        string Description,
        string PrincipalEvidenceDescription,
        Stream PrincipalEvidence,
        IReadOnlyList<string> GaleryEvidencesDescriptions,
        IReadOnlyList<Stream> GaleryEvidences);
}
