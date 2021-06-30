namespace Chami.Db.Entities
{
    /// <summary>
    /// Clarifies what type an environment is.
    /// These are:
    /// <list type="number">
    /// <item>
    /// Normal environment: this is an environment that was created by the user and can be customized.
    /// </item>
    /// <item>Backup environment: this is an environment that was created based on the current set of <see cref="EnvironmentVariable"/>s on the user's machine.</item>
    /// <item>Template environment: this is an environment that was created by the user in order to create other <see cref="NormalEnvironment"/> environments.</item>
    /// </list>
    /// </summary>
    public enum EnvironmentType
    {
        NormalEnvironment = 0,
        BackupEnvironment = 1,
        TemplateEnvironment = 2
    }
}