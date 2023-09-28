import 'package:bitsdojo_window/bitsdojo_window.dart';
import 'package:flutter/material.dart';

class TitleBar extends StatelessWidget {
  const TitleBar({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 36,
      color: Theme.of(context).primaryColor,
      child: Row(
        children: [
          Expanded(child: MoveWindow()),
          Row(children: [
            MinimizeWindowButton(colors: WindowButtonColors(iconNormal: Colors.white)),
            MaximizeWindowButton(colors: WindowButtonColors(iconNormal: Colors.white)),
            CloseWindowButton(colors: WindowButtonColors(iconNormal: Colors.white, mouseOver: const Color(0xFFD32F2F), mouseDown: const Color(0xFFB71C1C))),
          ]),
        ],
      ),
    );
  }
}
