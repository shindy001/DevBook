import 'package:bitsdojo_window/bitsdojo_window.dart';
import 'package:flutter/material.dart';
import 'package:flutter_client/ui/theme/dev_book_colors.dart';

import '../layout/layout.dart';

class TitleBar extends StatelessWidget implements PreferredSizeWidget {
  const TitleBar({super.key});

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(
            child: Container(
          color: DevBookColors.background,
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
