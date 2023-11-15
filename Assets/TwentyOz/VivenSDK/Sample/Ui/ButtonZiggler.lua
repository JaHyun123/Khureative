local TargetTextTmp

function awake()
    self:GetComponent(typeof(Button)).onClick:AddListener(Shake)
    TargetTextTmp = TargetText.gameObject:GetComponent(typeof(TMP_Text))
    
    self:SetRoomProp("Shake", 0)
    TargetTextTmp.text = "Shake Me!!"
end

local int = 0

function Shake()
    self:DoShake(1, 100, 20)
    int = int + 1
    self:SetRoomProp("Shake", int)
    self:GetRoomProp("Shake")
end

function onRoomProp(key,value)
    TargetTextTmp.text = "Shake Count: (" .. value .. ")"
end