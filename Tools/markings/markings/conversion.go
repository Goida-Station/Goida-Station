// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

package markings

func accessories_to_markings(accessories []SpriteAccessoryPrototype) ([]MarkingPrototype, error) {
	res := make([]MarkingPrototype, 65)

	for _, v := range accessories {
		marking, err := v.toMarking()
		if err != nil {
			return nil, err
		}

		res = append(res, *marking)
	}

	return res, nil
}
