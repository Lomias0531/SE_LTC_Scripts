using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.ModAPI;
using VRageMath;

class Whip:API
{
    #region In-game Script
    /*
    /         HOW DO I USE THIS?
    1. Place this script in a programmable block.
    2. Place some turrets on your ship.
    3. Place a seat on your ship.
    4. Place some text panels with "Radar" in their name somewhere.
    5. Enjoy!
    =================================================
        DO NOT MODIFY VARIABLES IN THE SCRIPT!
        USE THE CUSTOM DATA OF THIS PROGRAMMABLE BLOCK!
    =================================================
    HEY! DONT EVEN THINK ABOUT TOUCHING BELOW THIS LINE!
    */
    #region Fields
    enum TargetRelation : byte { Neutral = 0, Enemy = 1, Friendly = 2, Locked = 4 }
    const string INI_SECTION_GENERAL = "Radar - General";
    const string INI_RADAR_NAME = "Text surface name tag";
    const string INI_REF_NAME = "Optional reference block name";
    const string INI_BCAST = "Share own position";
    const string INI_NETWORK = "Share targets";
    const string INI_USE_RANGE_OVERRIDE = "Use radar range override";
    const string INI_RANGE_OVERRIDE = "Radar range override (m)";
    const string INI_PROJ_ANGLE = "Radar projection angle in degrees (0 is flat)";
    const string INI_DRAW_QUADRANTS = "Draw quadrants";
    const string INI_SECTION_COLORS = "Radar - Colors";
    const string INI_ENEMY = "Enemy icon";
    const string INI_ENEMY_ELEVATION = "Enemy elevation";
    const string INI_NEUTRAL = "Neutral icon";
    const string INI_NEUTRAL_ELEVATION = "Neutral elevation";
    const string INI_FRIENDLY = "Friendly icon";
    const string INI_FRIENDLY_ELEVATION = "Friendly elevation";
    const string INI_SECTION_TEXT_SURF_PROVIDER = "Radar - Text Surface Config";
    const string INI_TEXT_SURFACE_TEMPLATE = "Show on screen {0}";
    string referenceName = "Reference";
    float rangeOverride = 20000;
    bool useRangeOverride = false;
    bool networkTargets = true;
    bool broadcastIFF = true;
    bool drawQuadrants = true;
    Color enemyIconColor = new Color(150, 0, 0, 255);
    Color enemyElevationColor = new Color(75, 0, 0, 255);
    Color neutralIconColor = new Color(150, 150, 0, 255);
    Color neutralElevationColor = new Color(75, 75, 0, 255);
    Color allyIconColor = new Color(0, 50, 150, 255);
    Color allyElevationColor = new Color(0, 25, 75, 255);
    float MaxRange { get { return Math.Max(1, useRangeOverride ? rangeOverride : turretMaxRange); } }
    string textPanelName = "Radar";
    float projectionAngle = 60f;
    float turretMaxRange = 800f;
    Dictionary<long, TargetData> targetDataDict = new Dictionary<long, TargetData>();
    Dictionary<long, TargetData> broadcastDict = new Dictionary<long, TargetData>();
    List<IMySensorBlock> sensors = new List<IMySensorBlock>();
    List<IMyTextSurface> textSurfaces = new List<IMyTextSurface>();
    bool isSetup = false;
    bool _clearSpriteCache = false;
    readonly RadarSurface radarSurface;
    readonly MyIni generalIni = new MyIni();
    readonly MyIni textSurfaceIni = new MyIni();
    readonly MyCommandLine _commandLine = new MyCommandLine();
    #endregion
    #region Main Routine
    Program()
    {
        ParseCustomDataIni();
        GrabBlocks();
        radarSurface = new RadarSurface(projectionAngle, MaxRange, drawQuadrants);
        UpdateActions.Add(GetTurretTargets);
        UpdateActions.Add(radarSurface.SortContacts);
        float step = 1f / 8f;
        UpdateActions.Add(() => Draw(0 * step, 1 * step));
        UpdateActions.Add(() => Draw(1 * step, 2 * step));
        UpdateActions.Add(() => Draw(2 * step, 3 * step));
        UpdateActions.Add(() => Draw(3 * step, 4 * step));
        UpdateActions.Add(() => Draw(4 * step, 5 * step));
        UpdateActions.Add(() => Draw(5 * step, 6 * step));
        UpdateActions.Add(() => Draw(6 * step, 7 * step));
        UpdateActions.Add(() => Draw(7 * step, 8 * step));
        Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10 | UpdateFrequency.Update100;
    }
    List<Action> UpdateActions = new List<Action>();
    void Main(string arg, UpdateType updateSource)
    {
        if (_commandLine.TryParse(arg))
            HandleArguments();
        if (updateSource.HasFlag(UpdateType.Update100))
        {
            GrabBlocks();
        }
        if (updateSource.HasFlag(UpdateType.Update10))
        {
            UpdateRadarRange();
        }
        if (updateSource.HasFlag(UpdateType.Update1))
        {
            try
            {
                if (UpdateActions.Count < 1) return;
                UpdateActions[(count++) % UpdateActions.Count]();
            }
            catch (Exception) { }
        }
    }
    int count = 0;
    void HandleArguments()
    {
        int argCount = _commandLine.ArgumentCount;
        if (argCount == 0)
            return;
        switch (_commandLine.Argument(0).ToLowerInvariant())
        {
            case "range":
                if (argCount != 2)
                {
                    return;
                }
                float range = 0;
                if (float.TryParse(_commandLine.Argument(1), out range))
                {
                    useRangeOverride = true;
                    rangeOverride = range;
                    UpdateRadarRange();
                    generalIni.Clear();
                    generalIni.TryParse(Me.CustomData);
                    generalIni.Set(INI_SECTION_GENERAL, INI_RANGE_OVERRIDE, rangeOverride);
                    generalIni.Set(INI_SECTION_GENERAL, INI_USE_RANGE_OVERRIDE, useRangeOverride);
                    Me.CustomData = generalIni.ToString();
                }
                else if (string.Equals(_commandLine.Argument(1), "default"))
                {
                    useRangeOverride = false;
                    UpdateRadarRange();
                    generalIni.Clear();
                    generalIni.TryParse(Me.CustomData);
                    generalIni.Set(INI_SECTION_GENERAL, INI_USE_RANGE_OVERRIDE, useRangeOverride);
                    Me.CustomData = generalIni.ToString();
                }
                return;
            default:
                return;
        }
    }
    void Draw(float startProportion, float endProportion)
    {
        int start = (int)(startProportion * textSurfaces.Count);
        int end = (int)(endProportion * textSurfaces.Count);
        for (int i = start; i < end; ++i)
        {
            var textSurface = textSurfaces[i];
            radarSurface.DrawRadar(textSurface, _clearSpriteCache);
        }
    }
    void UpdateRadarRange()
    {
        radarSurface.Range = MaxRange;
        radarSurface.angle += (MathHelper.Pi / 36f);
        radarSurface.angle = MathHelper.WrapAngle(radarSurface.angle);
    }
    #endregion
    #region Sensor Detection
    readonly List<MyDetectedEntityInfo> sensorEntities = new List<MyDetectedEntityInfo>();
    void GetSensorTargets()
    {
        if (Common.IsNull(sensors)) return;
        sensors.RemoveAll(sensor => sensor == null || IsClosed(sensor));
        foreach (var sensor in sensors)
        {
            sensorEntities.Clear();
            sensor.DetectedEntities(sensorEntities);
            foreach (var target in sensorEntities)
                AddTargetData(target);
        }
    }
    #endregion
    #region Add Target Info
    void AddTargetData(MyDetectedEntityInfo targetInfo)
    {
        TargetData targetData;
        targetDataDict.TryGetValue(targetInfo.EntityId, out targetData);
        if (targetInfo.Relationship == MyRelationsBetweenPlayerAndBlock.Enemies)
        {
            targetData.Relation |= TargetRelation.Enemy;
        }
        else if ((targetInfo.Relationship & (MyRelationsBetweenPlayerAndBlock.Owner | MyRelationsBetweenPlayerAndBlock.Friends)) != 0)
        {
            targetData.Relation |= TargetRelation.Friendly;
        }
        else
        {
            targetData.Relation |= TargetRelation.Neutral;
        }
        targetData.Position = targetInfo.Position;
        targetDataDict[targetInfo.EntityId] = targetData;
        broadcastDict[targetInfo.EntityId] = targetData;
    }
    #endregion
    #region Turret Detection
    void GetTurretTargets()
    {
        if (!isSetup) return;
        broadcastDict.Clear();
        radarSurface.ClearContacts();
        GetSensorTargets();
        foreach (var kvp in targetDataDict)
        {
            if (kvp.Key == Me.CubeGrid.EntityId)
                continue;
            var targetData = kvp.Value;
            Color targetIconColor = enemyIconColor;
            Color targetElevationColor = enemyElevationColor;
            RadarSurface.Relation relation = RadarSurface.Relation.Hostile;
            switch (targetData.Relation)
            {
                case TargetRelation.Enemy:
                    break;
                case TargetRelation.Neutral:
                    targetIconColor = neutralIconColor;
                    targetElevationColor = neutralElevationColor;
                    relation = RadarSurface.Relation.Neutral;
                    break;
                case TargetRelation.Friendly:
                    targetIconColor = allyIconColor;
                    targetElevationColor = allyElevationColor;
                    relation = RadarSurface.Relation.Allied;
                    break;
            }
            radarSurface.AddContact(targetData.Position, Me.WorldMatrix, targetIconColor, targetElevationColor, relation, targetData.TargetLock);
        }
        targetDataDict.Clear();
        radarSurface.RadarLockWarning = false;
    }
    static bool IsClosed(IMyTerminalBlock block) => block.WorldMatrix == MatrixD.Identity;

    #endregion
    #region Block Fetching
    void GrabBlocks()
    {
        _clearSpriteCache = !_clearSpriteCache;
        sensors = Common.GetTs<IMySensorBlock>(GridTerminalSystem);
        var textsurfaces = Common.GetTs<IMyTerminalBlock>(GridTerminalSystem, b => b.IsSameConstructAs(Me) && (b is IMyTextSurface || b is IMyTextSurfaceProvider) && b.CustomName.Contains(textPanelName));
        foreach (var item in textsurfaces) AddTextSurfaces(item, textSurfaces);
        if (textSurfaces.Count == 0)
            isSetup = false;
        else
        {
            isSetup = true;
            ParseCustomDataIni();
        }
    }
    #endregion
    #region Ini stuff
    void AddTextSurfaces(IMyTerminalBlock block, List<IMyTextSurface> textSurfaces)
    {
        var textSurface = block as IMyTextSurface;
        if (textSurface != null)
        {
            textSurfaces.Add(textSurface);
            return;
        }
        var surfaceProvider = block as IMyTextSurfaceProvider;
        if (surfaceProvider == null)
            return;
        textSurfaceIni.Clear();
        bool parsed = textSurfaceIni.TryParse(block.CustomData);
        if (!parsed && !string.IsNullOrWhiteSpace(block.CustomData))
        {
            textSurfaceIni.EndContent = block.CustomData;
        }
        int surfaceCount = surfaceProvider.SurfaceCount;
        for (int i = 0; i < surfaceCount; ++i)
        {
            string iniKey = string.Format(INI_TEXT_SURFACE_TEMPLATE, i);
            bool display = textSurfaceIni.Get(INI_SECTION_TEXT_SURF_PROVIDER, iniKey).ToBoolean(i == 0 && !(block is IMyProgrammableBlock));
            if (display)
            {
                textSurfaces.Add(surfaceProvider.GetSurface(i));
            }
            textSurfaceIni.Set(INI_SECTION_TEXT_SURF_PROVIDER, iniKey, display);
        }
        string output = textSurfaceIni.ToString();
        if (!string.Equals(output, block.CustomData))
            block.CustomData = output;
    }
    void WriteCustomDataIni()
    {
        generalIni.Set(INI_SECTION_GENERAL, INI_RADAR_NAME, textPanelName);
        generalIni.Set(INI_SECTION_GENERAL, INI_BCAST, broadcastIFF);
        generalIni.Set(INI_SECTION_GENERAL, INI_NETWORK, networkTargets);
        generalIni.Set(INI_SECTION_GENERAL, INI_USE_RANGE_OVERRIDE, useRangeOverride);
        generalIni.Set(INI_SECTION_GENERAL, INI_RANGE_OVERRIDE, rangeOverride);
        generalIni.Set(INI_SECTION_GENERAL, INI_PROJ_ANGLE, projectionAngle);
        generalIni.Set(INI_SECTION_GENERAL, INI_DRAW_QUADRANTS, drawQuadrants);
        generalIni.Set(INI_SECTION_GENERAL, INI_REF_NAME, referenceName);
        MyIniHelper.SetColor(INI_SECTION_COLORS, INI_ENEMY, enemyIconColor, generalIni);
        MyIniHelper.SetColor(INI_SECTION_COLORS, INI_ENEMY_ELEVATION, enemyElevationColor, generalIni);
        MyIniHelper.SetColor(INI_SECTION_COLORS, INI_NEUTRAL, neutralIconColor, generalIni);
        MyIniHelper.SetColor(INI_SECTION_COLORS, INI_NEUTRAL_ELEVATION, neutralElevationColor, generalIni);
        MyIniHelper.SetColor(INI_SECTION_COLORS, INI_FRIENDLY, allyIconColor, generalIni);
        MyIniHelper.SetColor(INI_SECTION_COLORS, INI_FRIENDLY_ELEVATION, allyElevationColor, generalIni);
        generalIni.SetSectionComment(INI_SECTION_COLORS, "Colors are defined with RGBAlpha color codes where\nvalues can range from 0,0,0,0 [transparent] to 255,255,255,255 [white].");
        string output = generalIni.ToString();
        if (!string.Equals(output, Me.CustomData))
            Me.CustomData = output;
    }
    void ParseCustomDataIni()
    {
        generalIni.Clear();
        if (generalIni.TryParse(Me.CustomData))
        {
            textPanelName = generalIni.Get(INI_SECTION_GENERAL, INI_RADAR_NAME).ToString(textPanelName);
            referenceName = generalIni.Get(INI_SECTION_GENERAL, INI_REF_NAME).ToString(referenceName);
            broadcastIFF = generalIni.Get(INI_SECTION_GENERAL, INI_BCAST).ToBoolean(broadcastIFF);
            networkTargets = generalIni.Get(INI_SECTION_GENERAL, INI_NETWORK).ToBoolean(networkTargets);
            useRangeOverride = generalIni.Get(INI_SECTION_GENERAL, INI_USE_RANGE_OVERRIDE).ToBoolean(useRangeOverride);
            rangeOverride = generalIni.Get(INI_SECTION_GENERAL, INI_RANGE_OVERRIDE).ToSingle(rangeOverride);
            projectionAngle = generalIni.Get(INI_SECTION_GENERAL, INI_PROJ_ANGLE).ToSingle(projectionAngle);
            drawQuadrants = generalIni.Get(INI_SECTION_GENERAL, INI_DRAW_QUADRANTS).ToBoolean(drawQuadrants);
            enemyIconColor = MyIniHelper.GetColor(INI_SECTION_COLORS, INI_ENEMY, generalIni, enemyIconColor);
            enemyElevationColor = MyIniHelper.GetColor(INI_SECTION_COLORS, INI_ENEMY_ELEVATION, generalIni, enemyElevationColor);
            neutralIconColor = MyIniHelper.GetColor(INI_SECTION_COLORS, INI_NEUTRAL, generalIni, neutralIconColor);
            neutralElevationColor = MyIniHelper.GetColor(INI_SECTION_COLORS, INI_NEUTRAL_ELEVATION, generalIni, neutralElevationColor);
            allyIconColor = MyIniHelper.GetColor(INI_SECTION_COLORS, INI_FRIENDLY, generalIni, allyIconColor);
            allyElevationColor = MyIniHelper.GetColor(INI_SECTION_COLORS, INI_FRIENDLY_ELEVATION, generalIni, allyElevationColor);
        }
        else if (!string.IsNullOrWhiteSpace(Me.CustomData))
        {
            generalIni.EndContent = Me.CustomData;
        }
        WriteCustomDataIni();
        if (radarSurface != null)
        {
            radarSurface.UpdateFields(projectionAngle, MaxRange, drawQuadrants);
        }
    }

    #endregion

    #region Radar Surface
    public static class MyIniHelper
    {
        public static void SetColor(string sectionName, string itemName, Color color, MyIni ini)
        {
            string colorString = string.Format("{0}, {1}, {2}, {3}", color.R, color.G, color.B, color.A);
            ini.Set(sectionName, itemName, colorString);
        }
        public static Color GetColor(string sectionName, string itemName, MyIni ini, Color? defaultChar = null)
        {
            string rgbString = ini.Get(sectionName, itemName).ToString("null");
            string[] rgbSplit = rgbString.Split(',');
            int r = 0, g = 0, b = 0, a = 0;
            if (rgbSplit.Length != 4)
            {
                if (defaultChar.HasValue)
                    return defaultChar.Value;
                else
                    return Color.Transparent;
            }
            int.TryParse(rgbSplit[0].Trim(), out r);
            int.TryParse(rgbSplit[1].Trim(), out g);
            int.TryParse(rgbSplit[2].Trim(), out b);
            bool hasAlpha = int.TryParse(rgbSplit[3].Trim(), out a);
            if (!hasAlpha)
                a = 255;
            r = MathHelper.Clamp(r, 0, 255);
            g = MathHelper.Clamp(g, 0, 255);
            b = MathHelper.Clamp(b, 0, 255);
            a = MathHelper.Clamp(a, 0, 255);
            return new Color(r, g, b, a);
        }
    }
    struct TargetData
    {
        public Vector3D Position;
        public TargetRelation Relation;
        public bool TargetLock;
        public TargetData(Vector3D position, TargetRelation relation, bool targetLock = false)
        {
            Position = position;
            Relation = relation;
            TargetLock = targetLock;
        }
    }
    class RadarSurface
    {
        float _range = 0f;
        public float Range { get { return _range; } set { if (value == _range) return; _range = value; PrefixRangeWithMetricUnits(_range, "m", 1, out _outerRange); } }
        public bool RadarLockWarning { get; set; }
        public enum Relation { None = 0, Allied = 1, Neutral = 2, Hostile = 3 }
        public readonly StringBuilder Debug = new StringBuilder();
        const string FONT = "Debug";
        const string RADAR_WARNING_TEXT = "MISSILE LOCK";
        const string ICON_OUT_OF_RANGE = "AH_BoreSight";
        const float TITLE_TEXT_SIZE = 1.5f;
        const float RANGE_TEXT_SIZE = 1.2f;
        const float TGT_ELEVATION_LINE_WIDTH = 4f;
        const float QUADRANT_LINE_WIDTH = 2f;
        const float TITLE_BAR_HEIGHT = 64;
        const float RADAR_WARNING_TEXT_SIZE = 1.5f;
        public float angle = 0;
        Color _titleBarColor;
        Color _backColor;
        Color _lineColor;
        Color _quadrantLineColor;
        Color _planeColor;
        Color _textColor;
        float _projectionAngleDeg;
        float _radarProjectionCos;
        float _radarProjectionSin;
        bool _drawQuadrants;
        bool _showRadarWarning = true;
        string _outerRange = "";
        Vector2 _quadrantLineDirection;
        Color _radarLockWarningColor = Color.Red;
        Color _textBoxBackgroundColor = new Color(0, 0, 0, 220);
        readonly StringBuilder _textMeasuringSB = new StringBuilder();
        readonly Vector2 TGT_ICON_SIZE = new Vector2(20, 20);
        readonly Vector2 SHIP_ICON_SIZE = new Vector2(32, 16);
        readonly List<TargetInfo> _targetList = new List<TargetInfo>();
        readonly List<TargetInfo> _targetsBelowPlane = new List<TargetInfo>();
        readonly List<TargetInfo> _targetsAbovePlane = new List<TargetInfo>();
        readonly Dictionary<Relation, string> _spriteMap = new Dictionary<Relation, string>() { { Relation.None, "None" }, { Relation.Allied, "SquareSimple" }, { Relation.Neutral, "Triangle" }, { Relation.Hostile, "Circle" }, };
        struct TargetInfo
        {
            public Vector3 Position;
            public Color IconColor;
            public Color ElevationColor;
            public string Icon;
            public bool TargetLock;
            public float Rotation;
            public float Scale;
        }
        public RadarSurface(float projectionAngleDeg, float range, bool drawQuadrants)
        {
            UpdateFields(projectionAngleDeg, range, drawQuadrants);
            _textMeasuringSB.Append(RADAR_WARNING_TEXT);
        }
        public void UpdateFields(float projectionAngleDeg, float range, bool drawQuadrants)
        {
            _projectionAngleDeg = projectionAngleDeg;
            _drawQuadrants = drawQuadrants;
            Range = range;
            PrefixRangeWithMetricUnits(Range, "m", 2, out _outerRange);
            var rads = MathHelper.ToRadians(_projectionAngleDeg);
            _radarProjectionCos = (float)Math.Cos(rads);
            _radarProjectionSin = (float)Math.Sin(rads);
            _quadrantLineDirection = new Vector2(0.25f * MathHelper.Sqrt2, 0.25f * MathHelper.Sqrt2 * _radarProjectionCos);
        }
        public void DrawRadarLockWarning(MySpriteDrawFrame frame, IMyTextSurface surface, Vector2 screenCenter, Vector2 screenSize, float scale)
        {
            if (!RadarLockWarning || !_showRadarWarning)
                return;
            float textSize = RADAR_WARNING_TEXT_SIZE * scale;
            Vector2 textBoxSize = surface.MeasureStringInPixels(_textMeasuringSB, "Debug", textSize);
            Vector2 padding = new Vector2(48f, 24f) * scale;
            Vector2 position = screenCenter + new Vector2(0, screenSize.Y * 0.2f);
            Vector2 textPos = position;
            textPos.Y -= textBoxSize.Y * 0.5f;
            MySprite textBoxBg = new MySprite(SpriteType.TEXTURE, "SquareSimple", color: _textBoxBackgroundColor, size: textBoxSize + padding);
            textBoxBg.Position = position;
            frame.Add(textBoxBg);
            MySprite textBox = new MySprite(SpriteType.TEXTURE, "AH_TextBox", color: _radarLockWarningColor, size: textBoxSize + padding);
            textBox.Position = position;
            frame.Add(textBox);
            MySprite text = MySprite.CreateText(RADAR_WARNING_TEXT, "Debug", _radarLockWarningColor, scale: textSize);
            text.Position = textPos;
            frame.Add(text);
        }
        public void AddContact(Vector3D worldPosition, MatrixD worldMatrix, Color iconColor, Color elevationLineColor, Relation relation, bool targetLock)
        {
            Vector3D transformedDirection = Vector3D.TransformNormal(worldPosition - worldMatrix.Translation, Matrix.Transpose(worldMatrix));
            Vector3 position = new Vector3(transformedDirection.X, transformedDirection.Z, transformedDirection.Y);
            bool inRange = position.X * position.X + position.Y * position.Y < Range * Range;
            string spriteName = "";
            float angle = 0f;
            float scale = 1f;
            if (inRange)
            {
                _spriteMap.TryGetValue(relation, out spriteName);
                position /= Range;
            }
            else
            {
                spriteName = ICON_OUT_OF_RANGE;
                scale = 4f;
                var directionFlat = position;
                directionFlat.Z = 0;
                float angleOffset = position.Z > 0 ? MathHelper.Pi : 0f;
                position = Vector3D.Normalize(directionFlat);
                angle = angleOffset + MathHelper.PiOver2;
            }
            var targetInfo = new TargetInfo()
            {
                Position = position,
                ElevationColor = elevationLineColor,
                IconColor = iconColor,
                Icon = spriteName,
                TargetLock = targetLock,
                Rotation = angle,
                Scale = scale,
            };
            _targetList.Add(targetInfo);
        }
        public void SortContacts()
        {
            _targetsBelowPlane.Clear();
            _targetsAbovePlane.Clear();
            _targetList.Sort((a, b) => (a.Position.Y).CompareTo(b.Position.Y));
            foreach (var target in _targetList)
            {
                if (target.Position.Z >= 0)
                    _targetsAbovePlane.Add(target);
                else
                    _targetsBelowPlane.Add(target);
            }
            _showRadarWarning = !_showRadarWarning;
        }
        public void ClearContacts()
        {
            _targetList.Clear();
            _targetsAbovePlane.Clear();
            _targetsBelowPlane.Clear();
        }
        static void DrawBoxCorners(MySpriteDrawFrame frame, Vector2 boxSize, Vector2 centerPos, float lineLength, float lineWidth, Color color)
        {
            Vector2 horizontalSize = new Vector2(lineLength, lineWidth);
            Vector2 verticalSize = new Vector2(lineWidth, lineLength);
            Vector2 horizontalOffset = 0.5f * horizontalSize;
            Vector2 verticalOffset = 0.5f * verticalSize;
            Vector2 boxHalfSize = 0.5f * boxSize;
            Vector2 boxTopLeft = centerPos - boxHalfSize;
            Vector2 boxBottomRight = centerPos + boxHalfSize;
            Vector2 boxTopRight = centerPos + new Vector2(boxHalfSize.X, -boxHalfSize.Y);
            Vector2 boxBottomLeft = centerPos + new Vector2(-boxHalfSize.X, boxHalfSize.Y);
            MySprite sprite;
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: horizontalSize, position: boxTopLeft + horizontalOffset, rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: verticalSize, position: boxTopLeft + verticalOffset, rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: horizontalSize, position: boxTopRight + new Vector2(-horizontalOffset.X, horizontalOffset.Y), rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: verticalSize, position: boxTopRight + new Vector2(-verticalOffset.X, verticalOffset.Y), rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: horizontalSize, position: boxBottomLeft + new Vector2(horizontalOffset.X, -horizontalOffset.Y), rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: verticalSize, position: boxBottomLeft + new Vector2(verticalOffset.X, -verticalOffset.Y), rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: horizontalSize, position: boxBottomRight - horizontalOffset, rotation: 0, color: color);
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", size: verticalSize, position: boxBottomRight - verticalOffset, rotation: 0, color: color);
            frame.Add(sprite);
        }
        public void DrawRadar(IMyTextSurface surface, bool clearSpriteCache)
        {
            surface.ContentType = ContentType.SCRIPT;
            surface.Script = "";
            _textColor = _lineColor = surface.ScriptForegroundColor;
            _quadrantLineColor = _lineColor.ToVector3() / 2;
            _backColor = surface.ScriptBackgroundColor;
            var total_c = (_backColor.ToVector4() + _lineColor.ToVector4());
            _planeColor = new Color(Vector4.Clamp(total_c / 4, Vector4.Zero, Vector4.One * 0.2f));
            _titleBarColor = new Color(Vector4.Clamp(total_c / 2.5f, Vector4.Zero, Vector4.One * 0.25f));
            Vector2 surfaceSize = surface.TextureSize;
            Vector2 screenCenter = surfaceSize * 0.5f;
            Vector2 viewportSize = surface.SurfaceSize;
            Vector2 scale = viewportSize / 512f;
            float minScale = Math.Min(scale.X, scale.Y);
            float sideLength = Math.Min(viewportSize.X, viewportSize.Y - TITLE_BAR_HEIGHT * minScale);
            Vector2 radarCenterPos = screenCenter + Vector2.UnitY * (TITLE_BAR_HEIGHT * 0.5f * minScale);
            Vector2 radarPlaneSize = new Vector2(sideLength, sideLength * _radarProjectionCos);
            using (var frame = surface.DrawFrame())
            {
                if (clearSpriteCache)
                    frame.Add(new MySprite());
                DrawRadarPlane1(frame, viewportSize, screenCenter, radarCenterPos, radarPlaneSize, minScale);
                DrawRadarPlaneBackground(frame, radarCenterPos, radarPlaneSize);
                foreach (var targetInfo in _targetsBelowPlane)
                    DrawTargetIcon(frame, radarCenterPos, radarPlaneSize, targetInfo, minScale);
                DrawRadarPlane2(frame, viewportSize, screenCenter, radarCenterPos, radarPlaneSize, minScale);
                foreach (var targetInfo in _targetsAbovePlane)
                    DrawTargetIcon(frame, radarCenterPos, radarPlaneSize, targetInfo, minScale);
                DrawRadarLockWarning(frame, surface, screenCenter, viewportSize, minScale);
            }
        }
        void DrawLine(MySpriteDrawFrame frame, Vector2 point1, Vector2 point2, float width, Color color)
        {
            Vector2 position = 0.5f * (point1 + point2);
            Vector2 diff = point1 - point2;
            float length = diff.Length();
            if (length > 0)
                diff /= length;
            Vector2 size = new Vector2(length, width);
            float angle = (float)Math.Acos(Vector2.Dot(diff, Vector2.UnitX));
            angle *= Math.Sign(Vector2.Dot(diff, Vector2.UnitY));
            MySprite sprite = MySprite.CreateSprite("SquareSimple", position, size);
            sprite.RotationOrScale = angle;
            sprite.Color = color;
            frame.Add(sprite);
        }
        void DrawRadarPlaneBackground(MySpriteDrawFrame frame, Vector2 screenCenter, Vector2 radarPlaneSize)
        {
            MySprite sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize, color: _planeColor);
            sprite.Position = screenCenter;
            frame.Add(sprite);
        }
        void DrawRadarPlane1(MySpriteDrawFrame frame, Vector2 viewportSize, Vector2 screenCenter, Vector2 radarScreenCenter, Vector2 radarPlaneSize, float scale)
        {
            MySprite sprite;
            Vector2 halfScreenSize = viewportSize * 0.5f;
            float titleBarHeight = TITLE_BAR_HEIGHT * scale;
            sprite = MySprite.CreateSprite("SquareSimple",
                screenCenter + new Vector2(0f, -halfScreenSize.Y + titleBarHeight * 0.5f),
                new Vector2(viewportSize.X, titleBarHeight));
            sprite.Color = _titleBarColor;
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize + 3, color: _lineColor);
            sprite.Position = radarScreenCenter;
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize, color: _backColor);
            sprite.Position = radarScreenCenter;
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize * 0.5f + 3, color: _lineColor);
            sprite.Position = radarScreenCenter;
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize * 0.5f, color: _backColor);
            sprite.Position = radarScreenCenter;
            frame.Add(sprite);
            if (_drawQuadrants)
            {
                float lineWidth = QUADRANT_LINE_WIDTH * scale;
                Vector2 Pos_1 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radarPlaneSize / 2f;
                Vector2 Pos_2 = new Vector2((float)Math.Cos(angle + MathHelper.PiOver2), (float)Math.Sin(angle + MathHelper.PiOver2)) * radarPlaneSize / 2f;
                DrawLine(frame, radarScreenCenter + Pos_1, radarScreenCenter - Pos_1, lineWidth, _quadrantLineColor);
                DrawLine(frame, radarScreenCenter + Pos_2, radarScreenCenter - Pos_2, lineWidth, _quadrantLineColor);
                sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize * 0.15f, color: _backColor);
                sprite.Position = radarScreenCenter;
                frame.Add(sprite);
            }
            float textSize = RANGE_TEXT_SIZE * scale;
            Color rangeColors = new Color(_textColor.R, _textColor.G, _textColor.B, _textColor.A / 2);
            sprite = MySprite.CreateText($"Range: {_outerRange}", "Debug", rangeColors, textSize, TextAlignment.CENTER);
            sprite.Position = radarScreenCenter + new Vector2(0, radarPlaneSize.Y * 0.5f + scale * 4f);
            frame.Add(sprite);
        }
        void DrawRadarPlane2(MySpriteDrawFrame frame, Vector2 viewportSize, Vector2 screenCenter, Vector2 radarScreenCenter, Vector2 radarPlaneSize, float scale)
        {
            MySprite sprite;
            Vector2 halfScreenSize = viewportSize * 0.5f;
            float titleBarHeight = TITLE_BAR_HEIGHT * scale;
            sprite = MySprite.CreateSprite("SquareSimple",
                screenCenter + new Vector2(0f, -halfScreenSize.Y + titleBarHeight * 0.5f),
                new Vector2(viewportSize.X, titleBarHeight));
            sprite.Color = _titleBarColor;
            frame.Add(sprite);
            sprite = MySprite.CreateText($"WMI Radar System", FONT, _textColor, scale * TITLE_TEXT_SIZE, TextAlignment.CENTER);
            sprite.Position = screenCenter + new Vector2(0, -halfScreenSize.Y + 4.25f * scale);
            frame.Add(sprite);
            var iconSize = SHIP_ICON_SIZE * scale;
            sprite = new MySprite(SpriteType.TEXTURE, "Circle", size: radarPlaneSize + 3, color: _lineColor.ToVector4() / 3);
            sprite.Position = radarScreenCenter;
            frame.Add(sprite);
            sprite = new MySprite(SpriteType.TEXTURE, "Triangle", size: iconSize, color: _lineColor);
            sprite.Position = radarScreenCenter + new Vector2(0f, -0.2f * iconSize.Y);
            frame.Add(sprite);
            float textSize = RANGE_TEXT_SIZE * scale;
            Color rangeColors = new Color(_textColor.R, _textColor.G, _textColor.B, _textColor.A / 2);
            sprite = MySprite.CreateText($"Range: {_outerRange}", "Debug", rangeColors, textSize, TextAlignment.CENTER);
            sprite.Position = radarScreenCenter + new Vector2(0, radarPlaneSize.Y * 0.5f + scale * 4f);
            frame.Add(sprite);
        }
        void DrawTargetIcon(MySpriteDrawFrame frame, Vector2 screenCenter, Vector2 radarPlaneSize, TargetInfo targetInfo, float scale)
        {
            Vector3 targetPosPixels = targetInfo.Position * new Vector3(1, _radarProjectionCos, _radarProjectionSin) * radarPlaneSize.X * 0.5f;
            Vector2 targetPosPlane = new Vector2(targetPosPixels.X, targetPosPixels.Y);
            Vector2 iconPos = targetPosPlane - targetPosPixels.Z * Vector2.UnitY;
            RoundVector2(ref iconPos);
            RoundVector2(ref targetPosPlane);
            float elevationLineWidth = Math.Max(1f, TGT_ELEVATION_LINE_WIDTH * scale);
            MySprite elevationSprite = new MySprite(SpriteType.TEXTURE, "SquareSimple", color: targetInfo.ElevationColor, size: new Vector2(elevationLineWidth, targetPosPixels.Z));
            elevationSprite.Position = screenCenter + (iconPos + targetPosPlane) * 0.5f;
            RoundVector2(ref elevationSprite.Position);
            RoundVector2(ref elevationSprite.Size);
            Vector2 iconSize = TGT_ICON_SIZE * scale * targetInfo.Scale;
            MySprite iconSprite = new MySprite(SpriteType.TEXTURE, targetInfo.Icon, color: targetInfo.IconColor, size: iconSize, rotation: targetInfo.Rotation);
            iconSprite.Position = screenCenter + iconPos;
            RoundVector2(ref iconSprite.Position);
            RoundVector2(ref iconSprite.Size);
            MySprite iconShadow = iconSprite;
            iconShadow.Color = Color.Black;
            iconShadow.Size += Vector2.One * 2f * (float)Math.Max(1f, Math.Round(scale * 4f));
            iconSize.Y *= _radarProjectionCos;
            MySprite projectedIconSprite = new MySprite(SpriteType.TEXTURE, "Circle", color: targetInfo.ElevationColor, size: iconSize);
            projectedIconSprite.Position = screenCenter + targetPosPlane;
            RoundVector2(ref projectedIconSprite.Position);
            RoundVector2(ref projectedIconSprite.Size);
            bool showProjectedElevation = Math.Abs(iconPos.Y - targetPosPlane.Y) > iconSize.Y;
            if (targetPosPixels.Z >= 0)
            {
                if (showProjectedElevation)
                {
                    frame.Add(projectedIconSprite);
                    frame.Add(elevationSprite);
                }
                frame.Add(iconShadow);
                frame.Add(iconSprite);
            }
            else
            {
                iconSprite.RotationOrScale = MathHelper.Pi;
                iconShadow.RotationOrScale = MathHelper.Pi;
                if (showProjectedElevation)
                    frame.Add(elevationSprite);
                frame.Add(iconShadow);
                frame.Add(iconSprite);
                if (showProjectedElevation)
                    frame.Add(projectedIconSprite);
            }
            if (targetInfo.TargetLock)
            {
                DrawBoxCorners(frame, (TGT_ICON_SIZE + 20) * scale, screenCenter + iconPos, 12 * scale, 4 * scale, targetInfo.IconColor /*_targetLockColor*/);
            }
        }
        void RoundVector2(ref Vector2? vec)
        {
            if (vec.HasValue)
                vec = new Vector2((float)Math.Round(vec.Value.X), (float)Math.Round(vec.Value.Y));
        }
        void RoundVector2(ref Vector2 vec)
        {
            vec.X = (float)Math.Round(vec.X);
            vec.Y = (float)Math.Round(vec.Y);
        }
        string[] _prefixes = new string[] { "Y", "Z", "E", "P", "T", "G", "M", "k", };
        double[] _factors = new double[] { 1e24, 1e21, 1e18, 1e15, 1e12, 1e9, 1e6, 1e3, };
        void PrefixRangeWithMetricUnits(double num, string unit, int digits, out string numStr)
        {
            string prefix = "";
            for (int i = 0; i < _factors.Length; ++i)
            {
                double factor = _factors[i];
                if (num >= factor)
                {
                    prefix = _prefixes[i];
                    num /= factor;
                    break;
                }
            }
            numStr = (prefix == "" ? num.ToString("n0") : num.ToString($"n{digits}")) + $" {prefix}{unit}";
        }
    }
    static class Common
    {
        public static bool BlockInTurretGroup(IMyBlockGroup group, IMyTerminalBlock Me) { List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>(); group?.GetBlocks(blocks); if (blocks.Count < 1 || !blocks.Contains(Me)) return false; return true; }
        public static IMyTerminalBlock GetBlock(IMyGridTerminalSystem gridTerminalSystem, long EntIds = 0) => gridTerminalSystem?.GetBlockWithId(EntIds) as IMyTerminalBlock; public static List<IMyTerminalBlock> GetBlocks(IMyGridTerminalSystem gridTerminalSystem, List<long> EntIds = null) { if (gridTerminalSystem == null) return null; return EntIds?.ConvertAll(id => gridTerminalSystem.GetBlockWithId(id) as IMyTerminalBlock); }
        public static T GetT<T>(IMyGridTerminalSystem gridTerminalSystem, Func<T, bool> requst = null) where T : class { List<T> Items = GetTs(gridTerminalSystem, requst); if (IsNullC(Items)) return null; else return Items.First(); }
        public static T GetT<T>(IMyBlockGroup blockGroup, Func<T, bool> requst = null) where T : class => GetTs(blockGroup, requst).FirstOrDefault(); public static List<T> GetTs<T>(IMyGridTerminalSystem gridTerminalSystem, Func<T, bool> requst = null) where T : class { List<T> Items = new List<T>(); if (gridTerminalSystem == null) return Items; gridTerminalSystem.GetBlocksOfType(Items, requst); return Items; }
        public static List<T> GetTs<T>(IMyBlockGroup blockGroup, Func<T, bool> requst = null) where T : class { List<T> Items = new List<T>(); if (blockGroup == null) return Items; blockGroup.GetBlocksOfType(Items, requst); return Items; }
        public static Matrix GetWorldMatrix(IMyTerminalBlock ShipController) { Matrix me_matrix; ShipController.Orientation.GetMatrix(out me_matrix); return me_matrix; }
        public static IMyCameraBlock ID2Camera(IMyGridTerminalSystem GTS, long EntID) => GTS?.GetBlockWithId(EntID) as IMyCameraBlock; public static IMyMotorStator ID2Motor(IMyGridTerminalSystem GTS, long EntID) => GTS?.GetBlockWithId(EntID) as IMyMotorStator; public static IMyTerminalBlock ID2Weapon(IMyGridTerminalSystem GTS, long EntID) => GTS?.GetBlockWithId(EntID) as IMyTerminalBlock; public static bool IsNull(Vector3? Value) => Value == null || Value.Value == Vector3.Zero; public static bool IsNull(Vector3D? Value) => Value == null || Value.Value == Vector3D.Zero; public static bool IsNull<T>(T Value) where T : class => Value == null; public static bool IsNullC<T>(ICollection<T> Value) => (Value?.Count ?? 0) < 1; public static bool IsNullC<T>(IEnumerable<T> Value) => (Value?.Count() ?? 0) < 1; public static bool NullEntity<T>(T Ent) where T : IMyEntity => Ent == null; public static float SetInRange_AngularDampeners(float data) => MathHelper.Clamp(data, 0.01f, 20f); public static List<string> SpliteByQ(string context) => context?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? (new List<string>());
        public static bool StringContains(string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase) => source?.IndexOf(toCheck, comp) >= 0;
        public static void GetTextSurface(IMyTerminalBlock block, List<IMyTextSurface> TextSurfaces)
        {
            if (TextSurfaces == null) return;
            if (block is IMyTextSurface) { TextSurfaces.Add(block as IMyTextSurface); return; }
            if (block is IMyTextSurfaceProvider)
            {
                var TextSurfaceProvider = block as IMyTextSurfaceProvider;
                if (TextSurfaceProvider == null || TextSurfaceProvider.SurfaceCount < 1) return;
                for (int index = 0; index < TextSurfaceProvider.SurfaceCount; index++)
                    TextSurfaces.Add(TextSurfaceProvider.GetSurface(index));
                return;
            }
        }
    }

    #endregion

    #endregion
}
