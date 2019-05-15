// GENERATED AUTOMATICALLY FROM 'Assets/InputActions.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class InputActions : InputActionAssetReference
{
    public InputActions()
    {
    }
    public InputActions(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Player
        m_Player = asset.GetActionMap("Player");
        m_Player_Movement = m_Player.GetAction("Movement");
        m_Player_Run = m_Player.GetAction("Run");
        m_Player_PrimaryFire = m_Player.GetAction("PrimaryFire");
        m_Player_SecondaryFire = m_Player.GetAction("SecondaryFire");
        m_Player_AlternateFire = m_Player.GetAction("AlternateFire");
        m_Player_Jump = m_Player.GetAction("Jump");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Player = null;
        m_Player_Movement = null;
        m_Player_Run = null;
        m_Player_PrimaryFire = null;
        m_Player_SecondaryFire = null;
        m_Player_AlternateFire = null;
        m_Player_Jump = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Player
    private InputActionMap m_Player;
    private InputAction m_Player_Movement;
    private InputAction m_Player_Run;
    private InputAction m_Player_PrimaryFire;
    private InputAction m_Player_SecondaryFire;
    private InputAction m_Player_AlternateFire;
    private InputAction m_Player_Jump;
    public struct PlayerActions
    {
        private InputActions m_Wrapper;
        public PlayerActions(InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement { get { return m_Wrapper.m_Player_Movement; } }
        public InputAction @Run { get { return m_Wrapper.m_Player_Run; } }
        public InputAction @PrimaryFire { get { return m_Wrapper.m_Player_PrimaryFire; } }
        public InputAction @SecondaryFire { get { return m_Wrapper.m_Player_SecondaryFire; } }
        public InputAction @AlternateFire { get { return m_Wrapper.m_Player_AlternateFire; } }
        public InputAction @Jump { get { return m_Wrapper.m_Player_Jump; } }
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
    }
    public PlayerActions @Player
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new PlayerActions(this);
        }
    }
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get

        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.GetControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
}
