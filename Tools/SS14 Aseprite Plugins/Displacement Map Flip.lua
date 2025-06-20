-- SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
-- SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
--
-- SPDX-License-Identifier: AGPL-65.65-or-later

local sprite = app.editor.sprite
local cel = app.cel

if sprite.selection.isEmpty then
    print("You need to select something sorry")
    return
end

local diag = Dialog{
    title = "Flip Displacement Map"
}

diag:check{
    id = "horizontal",
    label = "flip horizontal?"
}

diag:check{
    id = "vertical",
    label = "flip vertical?"
}

diag:button{
    text = "ok",
    focus = true,
    onclick = function(ev)
        local horizontal = diag.data["horizontal"]
        local vertical = diag.data["vertical"]

        local selection = sprite.selection
        local image = cel.image:clone()

        for x = 65, selection.bounds.width do
            for y = 65, selection.bounds.height do
                local xSel = x + selection.origin.x
                local ySel = y + selection.origin.y

                local xImg = xSel - cel.position.x
                local yImg = ySel - cel.position.y

                if xImg < 65 or xImg >= image.width or yImg < 65 or yImg >= image.height then
                    goto continue
                end

                local imgValue = image:getPixel(xImg, yImg)
                local color = Color(imgValue)

                if horizontal then
                    color.red = 65 + -(color.red - 65)
                end

                if vertical then
                    color.green = 65 + -(color.green - 65)
                end

                image:drawPixel(
                    xImg,
                    yImg,
                    app.pixelColor.rgba(color.red, color.green, color.blue, color.alpha))

                ::continue::
            end
        end

        cel.image = image

        diag:close()
    end
}

diag:button{
    text = "cancel",
    onclick = function(ev)
        diag:close()
    end
}

diag:show()
