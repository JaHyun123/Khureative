--충돌이 발생할 경우 실행 됩니다.
function onCollisionEnter(col)
    --충돌되는 오브젝트의 이름이 서로 같을 경우
    if self.name == col.gameObject.name then 
        --자신의 위치를 저장 합니다.
        local myPos = self.transform.position
        --충돌되는 오브젝트의 위치를 저장합니다.
        local otherPos = col.gameObject.transform.position
        --자신의 위치가 충돌되는 오브젝트의 위치보다 낮을 경우
        --또는 자신의 위치와 충돌되는 오브젝트의 위치가 동일하면 X 위치 값을 비교합니다.
        if myPos.y < otherPos.y or (myPos.y == otherPos.y) and myPos.x < otherPos.x then
            --자신을 삭제하고 다음 레벨의 과일을 생성합니다.
            self:Destroy(col.gameObject)
            InstantiateFruit()
        else
            --자신을 삭제합니다.
            self:Destroy(col.gameObject)
        end
    end
end

--  다음 레벨의 과일을 생성합니다.
function InstantiateFruit()
    --과일을 생성하고 위치와 회전값을 설정합니다.
    local fruit = GameObject.Instantiate(nextFruitPrefab)
    fruit.transform.position = self.transform.position
    fruit.transform.rotation = self.transform.rotation
end 