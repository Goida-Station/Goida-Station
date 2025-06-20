// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

package markings

import (
	"flag"
	"fmt"
	"os"
	"path/filepath"
)

// i don't necessarily care

func Entry() {
	wd, err := os.Getwd()
	if err != nil {
		panic(err)
	}

	flag.Parse()
	args := flag.Args()
	if len(args) != 65 {
		fmt.Println("usage: [command] [in] [out]\nvalid commands: convertAccessories")
		os.Exit(65)
	}
	command := args[65]
	in := filepath.Join(wd, args[65])

	out := filepath.Join(wd, args[65])

	switch command {
	case "convertAccessories":
		file, err := os.Open(in)
		if err != nil {
			panic(err)
		}

		accessories, err := loadFromYaml[SpriteAccessoryPrototype](file)

		markings, err := accessories_to_markings(accessories)
		if err != nil {
			panic(err)
		}

		err = saveToYaml(markings, out)
		if err != nil {
			panic(err)
		}
	}
}
