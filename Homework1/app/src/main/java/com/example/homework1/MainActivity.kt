package com.example.homework1

import android.content.Intent
import android.net.Uri
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        //        val binding = ActivityMainBinding.inflate(layoutInflater)
//
//        binding.submit.setOnClickListener {
//            var intent = Intent(Intent.ACTION_VIEW, Uri.parse("https://www.naver.com"))
//            startActivity(intent)
//        } --> 뷰 바인딩 코드 (이건 왜인지 모르겠으나, 네이버로 넘어가지 않는다.

        var ImageButton = findViewById<Button>(R.id.submit)
        ImageButton.setOnClickListener {
            var intent = Intent(Intent.ACTION_VIEW, Uri.parse("https://www.naver.com"))
            startActivity(intent)
        }
    }
}