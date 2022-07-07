package com.example.boj_android

import android.graphics.Typeface
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.Gravity
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.TextView
import androidx.core.content.ContextCompat

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?)
    {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
//        val name = TextView(this).apply {
//            typeface = Typeface.DEFAULT_BOLD
//            text = "Lake Louise"
//        }
//        val image = ImageView(this).also{
//            it.setImageDrawable(ContextCompat.getDrawable(this, R.drawable.lake_1))
//        }
//        val address = TextView(this).apply{
//            typeface = Typeface.DEFAULT_BOLD
//            text = "Lake Louise, AB, 캐나다"
//        }
//        val layout = LinearLayout(this).apply{
//            orientation = LinearLayout.VERTICAL
//            gravity = Gravity.CENTER
//
//            addView(name, WRAP_CONTENT, WEAP_CONTENT)
//            addView(image, WRAP_CONTENT, WEAP_CONTENT)
//            addView(address, WRAP_CONTENT, WEAP_CONTENT)
//        }
//        setContentView(layout)
    }
}