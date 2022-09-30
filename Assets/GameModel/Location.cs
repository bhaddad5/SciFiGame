﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class Location : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;
		[TextArea(15, 20)]
		public string Description;

		public Texture2D Icon;
		public Texture2D BackgroundImage;
		public AudioClip OptionalBackgroundAudio = null;

		public enum LocationType
		{
			Misc,
			Office,
			Store,
			Home,
			Fun
		}

		public LocationType locationType;

		public Vector2 UiPosition;

		public List<Interaction> VisibilityInteractions;
		public List<Interaction> VisibilityNotCompletedInteractions;
		public bool ClosedOnWeekends;
		public bool ClosedOnWeekdays;

		public List<Policy> Policies = new List<Policy>();
		public List<Mission> Missions = new List<Mission>();
		public List<Npc> Npcs = new List<Npc>();

		public bool ShowTrophyCase;
		public bool ShowCar;
		public bool ShowMyOfficeCustomBackground;
		public bool ShowMyHome;

		[HideInInspector]
		public bool Controlled;

		public void Setup(MainGameManager mgm)
		{
			Controlled = false;

			foreach (var ob in Npcs)
				ob.Setup(mgm);
			foreach (var ob in Missions)
				ob.Setup();
			foreach (var ob in Policies)
				ob.Setup();
		}
		
		public bool IsVisible(MainGameManager mgm)
		{
			foreach (var interaction in VisibilityInteractions)
			{
				if (interaction != null && interaction.Completed == 0)
					return false;
			}
			foreach (var interaction in VisibilityNotCompletedInteractions)
			{
				if (interaction != null && interaction.Completed > 0)
					return false;
			}
			return true;
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Npcs.Any(n => n.IsVisible(mgm) && n.HasNewInteractions(mgm));
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			if (ClosedOnWeekdays && !mgm.IsWeekend())
				return false;
			if (ClosedOnWeekends && mgm.IsWeekend())
				return false;

			return true;
		}
	}
}