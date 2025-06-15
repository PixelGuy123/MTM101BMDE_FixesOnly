using System.IO;
using BepInEx;

namespace MTM101BaldAPI.AssetTools
{
  /// <summary>
	/// Provides utility methods for working with paths in a cross-platform manner.
	/// </summary>
  public static class PathX
  {
      /// <summary>
      /// Get a mod's mod path. (Currently StreamingAssets/Modded/[MOD GUID])
      /// </summary>
      /// <param name="plug"></param>
      /// <returns></returns>
      public static string GetModPath(BaseUnityPlugin plug)
      {
          return PathX.Combine(Application.streamingAssetsPath, "Modded", plug.Info.Metadata.GUID);
      }

      /// <summary>
  		/// Combines multiple path segments into a single path, handling case-insensitive directory and file matching. Useful for case-sensitive OS systems (eg. Linux).
  		/// </summary>
  		/// <param name="paths">An array of path segments to combine.</param>
  		/// <returns>The combined path.</returns>
  		/// <exception cref="ArgumentException">Thrown when no paths are provided.</exception>
  		/// <remarks>
  		/// This method attempts to find directories and files in a case-insensitive manner when combining paths.
  		/// If a path segment matches an existing directory or file (case-insensitively), it will use the actual path of the directory or file.
  		/// If no match is found, it will combine the path segments using standard <see cref="Path.Combine(string, string)"/> behavior.
  		/// </remarks>
  		public static string Combine(params string[] paths)
  		{
  			if (paths == null || paths.Length == 0)
  				throw new ArgumentException("No paths provided");
  
  			string currentPath = paths[0];
  			for (int i = 1; i < paths.Length; i++)
  			{
  				string nextPart = paths[i];
  				if (string.IsNullOrEmpty(nextPart)) continue;
  
  				// If currentPath is not a directory, just combine
  				if (!Directory.Exists(currentPath))
  				{
  					currentPath = Path.Combine(currentPath, nextPart);
  					continue;
  				}
  
  				// Case-insensitive directory search
  				string[] dirs = Directory.GetDirectories(currentPath);
  				string matchDir = Array.Find(dirs, d => string.Equals(Path.GetFileName(d), nextPart, StringComparison.OrdinalIgnoreCase)); // Neat! Array has a Find method
  				if (matchDir != null)
  				{
  					currentPath = matchDir;
  					continue;
  				}
  
  				// If not a directory, try as file
  				string[] files = Directory.GetFiles(currentPath);
  				string matchFile = Array.Find(files, f => string.Equals(Path.GetFileName(f), nextPart, StringComparison.OrdinalIgnoreCase));
  				if (matchFile != null)
  				{
  					currentPath = matchFile;
  					continue;
  				}
  
  				// Fallback: just combine
  				currentPath = Path.Combine(currentPath, nextPart);
  			}
  			return currentPath;
  		}
  }
}
