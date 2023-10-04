import 'package:flutter/material.dart';

import 'dev_book_colors.dart';
import 'dev_book_text_styles.dart';

class DevBookTheme {
  static ThemeData get themeDataDark {
    return ThemeData.dark().copyWith(
      useMaterial3: true,
      colorScheme: _colorScheme,
      scaffoldBackgroundColor: DevBookColors.backgroundMedium,
      textTheme: _textTheme.apply(
        bodyColor: DevBookColors.white,
        displayColor: DevBookColors.white,
        decorationColor: DevBookColors.white,
      ),
      listTileTheme: _listTileTheme,
      dialogTheme: _dialogTheme,
      bottomNavigationBarTheme: _bottomNavigationBarTheme,
      cardTheme: _cardTheme,
    );
  }

  static ColorScheme get _colorScheme {
    return ColorScheme.fromSeed(
      seedColor: DevBookColors.seedPurple,
      background: DevBookColors.backgroundMedium,
    );
  }

  static TextTheme get _textTheme {
    return DevBookTextStyles.desktop.textTheme;
  }

  static ListTileThemeData get _listTileTheme {
    return const ListTileThemeData(
      textColor: DevBookColors.grey,
      iconColor: DevBookColors.grey,
      selectedColor: DevBookColors.seedPurple,
    );
  }

  static DialogTheme get _dialogTheme {
    return const DialogTheme(
      backgroundColor: DevBookColors.black,
      surfaceTintColor: DevBookColors.transparent,
    );
  }

  static BottomNavigationBarThemeData get _bottomNavigationBarTheme {
    return const BottomNavigationBarThemeData(
      backgroundColor: DevBookColors.transparent,
    );
  }

  static CardTheme get _cardTheme {
    return const CardTheme(
      color: DevBookColors.backgroundLight,
    );
  }
}
