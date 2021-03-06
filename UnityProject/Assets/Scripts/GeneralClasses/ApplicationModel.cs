//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34209
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;


namespace AssemblyCSharp
{
	public class ApplicationModel
	{
		private static string currentLevelKey = "scoutForever_currentLevel";
		private static string currentScoutsRemainingKey = "scoutForever_currentScoutsRemaining";
		private static string totalScoutsSavedKey = "scoutForever_totalScoutsSaved";

		// Current Game Data
		public static string currentLevel;
		public static int scoutsRemaining;

		// Global Data
		public static int totalScoutsSaved;

		public static int totalScoutsHitByCar;
		public static int totalScoutsHurtByAxe;
		public static int totalScoutsLostInForest;

		public static int idealFontSize;

		public static void SaveData()
		{
			PlayerPrefs.SetString (currentLevelKey, currentLevel);
			PlayerPrefs.SetInt (currentScoutsRemainingKey, scoutsRemaining);
			PlayerPrefs.SetInt (totalScoutsSavedKey, totalScoutsSaved);
		}

		public static void LoadData(bool reinitCurrentGame)
		{
			if (reinitCurrentGame)
			{
				currentLevel = "";
				scoutsRemaining = 11;
			}
			else
			{
				currentLevel = PlayerPrefs.GetString (currentLevelKey);
				scoutsRemaining = PlayerPrefs.GetInt (currentScoutsRemainingKey);
			}
			totalScoutsSaved = PlayerPrefs.GetInt (totalScoutsSavedKey);
		}
	}
}

