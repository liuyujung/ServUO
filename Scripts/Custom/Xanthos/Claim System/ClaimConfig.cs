#region AuthorHeader
//
//	Claim System version 2.1, by Xanthos
//
//
#endregion AuthorHeader
using System;
using System.IO;  
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Server;
using Server.Items;
using Xanthos.Utilities;

namespace Xanthos.Claim
{
	// This file is for configuration of the Claim System.  It is advised
	// that you DO NOT edit this file, instead place ClaimConfig.xml in the 
	// RunUO/Data directory and modify the values there to configure the system
	// without changing code.  This allows you to take updates to the system
	// without losing your specific configuration settings.

	public class ClaimConfig
	{
		public static bool LootArtifacts = true;
		public static bool LootPlayers = false; 
		public static bool EnableGrab = true; 
		public static bool EnableClaim = true; 
		public static int GrabRadius = 3;
		public static int ClaimRadius = 15;
		public static int CompetitiveGrabRadius = 5;
		public static int CompetitiveClaimRadius = 10;
		public static int FreelyLootableFame = 1000;
		public static int FameDivisor = 100;
		public static int MinimumReward = 100;
		public static Type GoldBagType = typeof( Xanthos.Claim.GoldBag );
		public static bool AggregateSilver = false;
		public static Type SilverBagType = typeof( Xanthos.Claim.SilverBag );
		public static Type LootBagType = typeof( Xanthos.Claim.LootBag );
		public static bool GoldBagBlessed = true;
		public static bool SilverBagBlessed = false;
		public static bool LootBagBlessed = false;


		public static Type[] TypesToLoot = new Type []
		{
			typeof( Server.Items.Gold ),
		};

		private const string kConfigFile = @"Data/ClaimConfig.xml";
		private const string kConfigName = "ClaimSystem";

		public static void Initialize()
		{
			Element element = ConfigParser.GetConfig( kConfigFile, kConfigName );

			if ( null == element || element.ChildElements.Count <= 0 )
				return;

			bool tempBool;
			int tempInt;
			Type[] tempTypeArray;
			Type tempType;

			foreach( Element child in element.ChildElements ) 
			{
				if ( child.TagName == "LootArtifacts" && child.GetBoolValue( out tempBool ))
					LootArtifacts = tempBool;
				
				else if ( child.TagName == "EnableGrab" && child.GetBoolValue( out tempBool ))
					EnableGrab = tempBool;
				
				else if ( child.TagName == "EnableClaim" && child.GetBoolValue( out tempBool ))
					EnableClaim = tempBool;
				
				else if ( child.TagName == "LootPlayers" && child.GetBoolValue( out tempBool ))
					LootPlayers = tempBool;
				
				else if ( child.TagName == "GrabRadius" && child.GetIntValue( out tempInt ))
					GrabRadius = tempInt;
								
				else if ( child.TagName == "ClaimRadius" && child.GetIntValue( out tempInt ))
					ClaimRadius = tempInt;
								
				else if ( child.TagName == "CompetitiveGrabRadius" && child.GetIntValue( out tempInt ))
					CompetitiveGrabRadius = tempInt;
								
				else if ( child.TagName == "CompetitiveClaimRadius" && child.GetIntValue( out tempInt ))
					CompetitiveClaimRadius = tempInt;
								
				else if ( child.TagName == "FreelyLootableFame" && child.GetIntValue( out tempInt ))
					FreelyLootableFame = tempInt;

				else if ( child.TagName == "GoldBagType" && child.GetTypeValue( out tempType ) )
					GoldBagType = tempType;

				else if ( child.TagName == "SilverBagType" && child.GetTypeValue( out tempType ) )
					SilverBagType = tempType;

				else if ( child.TagName == "LootBagType" && child.GetTypeValue( out tempType ) )
					LootBagType = tempType;

				else if ( child.TagName == "AggregateSilver" && child.GetBoolValue( out tempBool ) )
					AggregateSilver = tempBool;

				else if ( child.TagName == "TypesToLoot" && child.GetArray( out tempTypeArray ) )
					TypesToLoot = tempTypeArray;

				else if ( child.TagName == "GoldBagBlessed" && child.GetBoolValue( out tempBool ) )
					GoldBagBlessed = tempBool;

				else if ( child.TagName == "SilverBagBlessed" && child.GetBoolValue( out tempBool ) )
					SilverBagBlessed = tempBool;

				else if ( child.TagName == "LootBagBlessed" && child.GetBoolValue( out tempBool ) )
					LootBagBlessed = tempBool;
			}
		}
	}
}