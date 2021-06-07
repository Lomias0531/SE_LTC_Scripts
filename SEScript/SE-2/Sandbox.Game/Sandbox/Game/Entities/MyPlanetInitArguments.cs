using Sandbox.Definitions;
using VRage.Game.Voxels;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	public struct MyPlanetInitArguments
	{
		public string StorageName;

		public int Seed;

		public IMyStorage Storage;

		public Vector3D PositionMinCorner;

		public float Radius;

		public float AtmosphereRadius;

		public float MaxRadius;

		public float MinRadius;

		public bool HasAtmosphere;

		public Vector3 AtmosphereWavelengths;

		public float GravityFalloff;

		public bool MarkAreaEmpty;

		public MyAtmosphereSettings AtmosphereSettings;

		public float SurfaceGravity;

		public bool AddGps;

		public bool SpherizeWithDistance;

		public MyPlanetGeneratorDefinition Generator;

		public bool UserCreated;

		public bool InitializeComponents;

		public bool FadeIn;

		public override string ToString()
		{
			return string.Concat("Planet init arguments: \nStorage name: ", StorageName ?? "<null>", "\n Storage: ", (Storage != null) ? Storage.ToString() : "<null>", "\n PositionMinCorner: ", PositionMinCorner, "\n Radius: ", Radius, "\n AtmosphereRadius: ", AtmosphereRadius, "\n MaxRadius: ", MaxRadius, "\n MinRadius: ", MinRadius, "\n HasAtmosphere: ", HasAtmosphere.ToString(), "\n AtmosphereWavelengths: ", AtmosphereWavelengths, "\n GravityFalloff: ", GravityFalloff, "\n MarkAreaEmpty: ", MarkAreaEmpty.ToString(), "\n AtmosphereSettings: ", AtmosphereSettings.ToString(), "\n SurfaceGravity: ", SurfaceGravity, "\n AddGps: ", AddGps.ToString(), "\n SpherizeWithDistance: ", SpherizeWithDistance.ToString(), "\n Generator: ", (Generator != null) ? Generator.ToString() : "<null>", "\n UserCreated: ", UserCreated.ToString(), "\n InitializeComponents: ", InitializeComponents.ToString());
		}
	}
}
