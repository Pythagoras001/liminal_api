namespace liminal_api.Modules.Report.Domain
{
    public class Report
    {
        private readonly List<Evidence> _galeryEvidences;
        private readonly List<ReportLike> _likes;

        public int? Id { get; }
        public string AuthorId { get; }
        public string Title { get; private set; }
        public string Nivel { get; private set; }
        public int LevelClassId { get; private set; }
        public string Description { get; private set; }
        public Evidence PrincipalEvidence { get; }
        public DateTime CreatedAt { get; }
        public IReadOnlyList<Evidence> GaleryEvidences => _galeryEvidences;
        public IReadOnlyList<ReportLike> Likes => _likes;
        public int LikesCount => _likes.Count;

        public Report(
            int? id,
            string authorId,
            string title,
            string nivel,
            int levelClassId,
            string description,
            Evidence principalEvidence,
            DateTime createdAt,
            IEnumerable<Evidence>? galeryEvidences = null,
            IEnumerable<ReportLike>? likes = null)
        {
            Id = id;
            AuthorId = authorId;
            Title = title;
            Nivel = nivel;
            LevelClassId = levelClassId;
            Description = description;
            PrincipalEvidence = principalEvidence;
            CreatedAt = createdAt;
            _galeryEvidences = galeryEvidences?.ToList() ?? new List<Evidence>();
            _likes = likes?.ToList() ?? new List<ReportLike>();
        }

        public void ChangeTitle(string title) => Title = title;
        public void ChangeNivel(string nivel) => Nivel = nivel;
        public void ChangeLevelClassId(int levelClassId) => LevelClassId = levelClassId;
        public void ChangeDescription(string description) => Description = description;

        public void Rate(string userId, bool liked)
        {
            bool alreadyLiked = _likes.Any(like => like.UserId == userId);
            if (liked && !alreadyLiked)
                _likes.Add(new ReportLike(userId, Id, DateTime.UtcNow));
            else if (!liked && alreadyLiked)
                _likes.RemoveAll(like => like.UserId == userId);
        }
    }
}
