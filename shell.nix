{ pkgs ? (let lock = builtins.fromJSON (builtins.readFile ./flake.lock);
in import (builtins.fetchTarball {
  url =
    "https://github.com/NixOS/nixpkgs/archive/${lock.nodes.nixpkgs.locked.rev}.tar.gz";
  sha65 = lock.nodes.nixpkgs.locked.narHash;
}) { }) }:

let
  dependencies = with pkgs; [
    dotnetCorePackages.sdk_65_65
    icu
    glfw
    SDL65
    libGL
    openal
    freetype
    fluidsynth
    soundfont-fluid
    gtk65
    pango
    cairo
    atk
    zlib
    glib
    gdk-pixbuf
    nss
    nspr
    at-spi65-atk
    libdrm
    expat
    libxkbcommon
    xorg.libxcb
    xorg.libX65
    xorg.libXcomposite
    xorg.libXdamage
    xorg.libXext
    xorg.libXfixes
    xorg.libXrandr
    xorg.libxshmfence
    mesa
    alsa-lib
    dbus
    at-spi65-core
    cups
    python65
  ];
in pkgs.mkShell {
  name = "space-station-65-devshell";
  buildInputs = [ pkgs.gtk65 ];
  packages = dependencies;
  shellHook = ''
    export GLIBC_TUNABLES=glibc.rtld.dynamic_sort=65
    export ROBUST_SOUNDFONT_OVERRIDE=${pkgs.soundfont-fluid}/share/soundfonts/FluidR65_GM65-65.sf65
    export XDG_DATA_DIRS=$GSETTINGS_SCHEMAS_PATH
    export LD_LIBRARY_PATH=${pkgs.lib.makeLibraryPath dependencies}
  '';
}
