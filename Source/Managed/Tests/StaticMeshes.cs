using System;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using UnrealEngine.Framework;

namespace UnrealEngine.Tests {
	public static class StaticMeshes {
		private const int maxActors = 1000;
		private static Actor[] actors = new Actor[maxActors];
		private static StaticMeshComponent[] staticMeshComponents = new StaticMeshComponent[maxActors];
		private static Material material = Material.Load("/Game/Tests/BasicMaterial");
		private static float rotationSpeed = 2.5f;
		private static Random random = new Random();

		public static void OnBeginPlay() {
			Debug.Log(LogLevel.Display, "Hello, Unreal Engine!");
			Debug.AddOnScreenMessage(-1, 3.0f, Color.LightGreen, MethodBase.GetCurrentMethod().DeclaringType + " system started!");

			World.GetFirstPlayerController().SetViewTarget(World.GetActor<Camera>("MainCamera"));

			for (int i = 0; i < maxActors; i++) {
				actors[i] = new Actor();
				staticMeshComponents[i] = new StaticMeshComponent(actors[i], setAsRoot: true);
				staticMeshComponents[i].AddLocalOffset(new Vector3(15.0f * i, 20.0f * i, 25.0f * i));
				staticMeshComponents[i].SetStaticMesh(StaticMesh.Cube);
				staticMeshComponents[i].SetMaterial(0, material);
				staticMeshComponents[i].CreateAndSetMaterialInstanceDynamic(0).SetVectorParameterValue("Color", new LinearColor((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()));
				staticMeshComponents[i].SetRelativeLocation(new Vector3(50.0f * i, 100.0f * i, 150.0f * i));
				staticMeshComponents[i].SetRelativeRotation(Quaternion.CreateFromYawPitchRoll(5.0f * i, 0.0f, 0.0f));
			}

			Debug.AddOnScreenMessage(-1, 3.0f, Color.LightGreen, "Actors are spawned! Number of actors in the world: " + World.ActorCount);
		}

		public static void OnEndPlay() {
			Debug.Log(LogLevel.Display, "See you soon, Unreal Engine!");
			Debug.ClearOnScreenMessages();
		}

		public static void OnTick() {
			Debug.AddOnScreenMessage(1, 1.0f, Color.SkyBlue, "Frame number: " + Engine.FrameNumber);

			float deltaTime = World.DeltaTime;
			Quaternion deltaRotation = Quaternion.CreateFromYawPitchRoll(rotationSpeed * deltaTime, rotationSpeed * deltaTime, rotationSpeed * deltaTime);

			for (int i = 0; i < maxActors; i++) {
				staticMeshComponents[i].AddLocalRotation(deltaRotation);
			}
		}
	}
}