using UnityEngine;
using System.Collections;

public class Events 
{
    public static class GameEvents
    {
        public static string GameStart = "GameStart";
        public static string RetryLevel = "RetryLevel";

        public static string ShowObjective = "ShowObjective";
        public static string ShowGameOver = "ShowGameOver";
        public static string ShowSettings = "ShowSettings";

        public static string ObjectiveShown = "ObjectiveShown";

        public static string IncrementScore = "IncrementScore";
        public static string IncrementMultipler = "IncrementMultiplier";

        public static string PlayerSpeedUpdated = "PlayerSpeedUpdated";

        public static string TriggerAfterburner = "TriggerAfterburner";
        public static string AfterburnerTriggered = "AfterburnerTriggered";

        public static string PlayerDied = "PlayerDied";

        public static string NewGameStarted = "NewGameStarted";

        public static string PlayerMoved = "PlayerMoved";

        public static string PostGameOverScore = "PostGameOverScore";

    }

    public static class MenuEvents
    {
        public static string TriggerPauseMenu = "TriggerPauseMenu";
        public static string TriggerSettingsMenu = "TriggerSettingsMenu";
    }

    public static class OnlineEvents
    {
        public static string TryAuthenticateUser = "TryAuthenticateUser";
    }

    public static class UIEvents
    {
        public static string MovesUpdated = "MovesUpdated";
        public static string HighScoreUpdated = "HighScoreUpdated";
        public static string MultiplierUpdated = "MultiplierUpdated";
        public static string FuelGaugeUpdated = "FuelGaugeUpdated";
        public static string RemainingLivesUpdated = "RemainingLivesUpdated";
        public static string ScoreUpdated = "ScoreUpdated";

        public static string ToggleTutorialScreen = "ToggleTutorialScreen";

    }

    public static class StoreEvents
    {
        public static string StoreInitialized = "StoreInitialized";
        public static string PurchaseNoAds = "PurchaseNoAds";
        public static string NoAdsPurchased = "NoAdsPurchased";
    }
}
