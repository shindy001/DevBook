import 'package:bitsdojo_window/bitsdojo_window.dart';
import 'package:flutter/material.dart';
import 'package:flutter_client/ui/theme/dev_book_colors.dart';

import '../../ui/layout/layout.dart';

class TitleBar extends StatelessWidget implements PreferredSizeWidget {
  const TitleBar({super.key});

  @override
  Widget build(BuildContext context) {
    final compactMenu = MediaQuery.of(context).size.width < DevBookBreakpoints.small;
    return Row(
      children: [
        Container(
          color: DevBookColors.backgroundDark,
          width: compactMenu ? DevBookSizes.compactMenuWidth : DevBookSizes.standardMenuWidth,
        ),
        Expanded(
            child: Container(
          color: DevBookColors.backgroundMedium,
          height: DevBookSizes.titleBarHeight,
          child: Row(
            children: [
              Expanded(child: MoveWindow()),
              Row(children: [
                MinimizeWindowButton(colors: WindowButtonColors(iconNormal: DevBookColors.white)),
                MaximizeWindowButton(colors: WindowButtonColors(iconNormal: DevBookColors.white)),
                CloseWindowButton(
                  colors: WindowButtonColors(
                    iconNormal: DevBookColors.white,
                    mouseOver: DevBookColors.closeWindowButtonMouseOver,
                    mouseDown: DevBookColors.closeWindowButtonMouseDown,
                  ),
                ),
              ]),
            ],
          ),
        )),
      ],
    );
  }

  @override
  Size get preferredSize => const Size.fromHeight(DevBookSizes.titleBarHeight);
}
